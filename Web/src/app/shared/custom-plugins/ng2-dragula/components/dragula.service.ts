import { Injectable, Optional } from '@angular/core';
import { Group } from '../Group';
import { DragulaOptions } from '../DragulaOptions';
import { Subject, Observable } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { EventTypes, AllEvents } from '../EventTypes';
import { DrakeFactory } from '../DrakeFactory';

type FilterProjector<T extends { name: string }> = (
  name: string,
  args: any[]
) => T;
interface Dispatch {
  event: EventTypes;
  name: string;
  args: any[];
}

const filterEvent = <T extends { name: string }>(
  eventType: EventTypes,
  filterDragType: string | undefined,
  projector: FilterProjector<T>
) => (input: Observable<Dispatch>): Observable<T> => {
  return input.pipe(
    filter(({ event, name }) => {
      return (
        event === eventType &&
        (filterDragType === undefined || name === filterDragType)
      );
    }),
    map(({ name, args }) => projector(name, args))
  );
};

const elContainerSourceProjector = (
  name: string,
  [el, container, source]: [Element, Element, Element]
) => ({ name, el, container, source });

@Injectable()
export class DragulaService {
  cancel = this._subService.elContainerSource(EventTypes.Cancel);
  remove = this._subService.elContainerSource(EventTypes.Remove);
  shadow = this._subService.elContainerSource(EventTypes.Shadow);
  over = this._subService.elContainerSource(EventTypes.Over);
  out = this._subService.elContainerSource(EventTypes.Out);

  constructor(@Optional() private drakeFactory: DrakeFactory = null, private _subService: SubDragulaService) {
    if (this.drakeFactory === null) {
      this.drakeFactory = new DrakeFactory();
    }
  }

  /* https://github.com/bevacqua/dragula#drakeon-events */

  private dispatch$ = new Subject<Dispatch>();

  private groups: { [k: string]: Group } = {};

  drag = (groupName?: string) =>
    this.dispatch$.pipe(
      filterEvent(
        EventTypes.Drag,
        groupName,
        (name, [el, source]: [Element, Element]) => ({ name, el, source })
      )
    )

  dragend = (groupName?: string) =>
    this.dispatch$.pipe(
      filterEvent(EventTypes.DragEnd, groupName, (name, [el]: [Element]) => ({
        name,
        el,
      }))
    )

  drop = (groupName?: string) =>
    this.dispatch$.pipe(
      filterEvent(
        EventTypes.Drop,
        groupName,
        (
          name,
          [el, target, source, sibling]: [Element, Element, Element, Element]
        ) => {
          return { name, el, target, source, sibling };
        }
      )
    )

  cloned = (groupName?: string) =>
    this.dispatch$.pipe(
      filterEvent(
        EventTypes.Cloned,
        groupName,
        (
          name,
          [clone, original, cloneType]: [Element, Element, 'mirror' | 'copy']
        ) => {
          return { name, clone, original, cloneType };
        }
      )
    )

  dropModel = <T = any>(groupName?: string) =>
    this.dispatch$.pipe(
      filterEvent(
        EventTypes.DropModel,
        groupName,
        (
          name,
          [
            el,
            target,
            source,
            sibling,
            item,
            sourceModel,
            targetModel,
            sourceIndex,
            targetIndex,
          ]: [Element, Element, Element, Element, T, T[], T[], number, number]
        ) => {
          return {
            name,
            el,
            target,
            source,
            sibling,
            item,
            sourceModel,
            targetModel,
            sourceIndex,
            targetIndex,
          };
        }
      )
    )

  removeModel = <T = any>(groupName?: string) =>
    this.dispatch$.pipe(
      filterEvent(
        EventTypes.RemoveModel,
        groupName,
        (
          name,
          [el, container, source, item, sourceModel, sourceIndex]: [
            Element,
            Element,
            Element,
            T,
            T[],
            number
          ]
        ) => {
          return {
            name,
            el,
            container,
            source,
            item,
            sourceModel,
            sourceIndex,
          };
        }
      )
    )

  /** Public mainly for testing purposes. Prefer `createGroup()`. */
  add(group: Group): Group {
    const existingGroup = this.find(group.name);
    if (existingGroup) {
      throw new Error('Group named: "' + group.name + '" already exists.');
    }
    this.groups[group.name] = group;
    this.handleModels(group);
    this.setupEvents(group);
    return group;
  }

  find(name: string): Group {
    return this.groups[name];
  }

  destroy(name: string): void {
    const group = this.find(name);
    if (!group) {
      return;
    }
    // else {
    //   group.drake && group.drake.destroy();
    //   delete this.groups[name];
    // }
  }

  /**
   * Creates a group with the specified name and options.
   *
   * Note: formerly known as `setOptions`
   */
  createGroup<T = any>(name: string, options: DragulaOptions<T>): Group {
    return this.add(
      new Group(name, this.drakeFactory.build([], options), options)
    );
  }
  // private elContainerSource = (eventType: EventTypes) => (groupName?: string) =>
  //   this.dispatch$.pipe(
  //     filterEvent(eventType, groupName, elContainerSourceProjector)
  //   )

  private handleModels({ name, drake, options }: Group): void {
    let dragElm: any;
    let dragIndex: number;
    let dropIndex: number;
    drake.on('remove', (el: any, container: any, source: any) => {
      if (!drake.models) {
        return;
      }
      let sourceModel = drake.models[drake.containers.indexOf(source)];
      sourceModel = sourceModel.slice(0); // clone it
      const item = sourceModel.splice(dragIndex, 1)[0];
      this.dispatch$.next({
        event: EventTypes.RemoveModel,
        name,
        args: [el, container, source, item, sourceModel, dragIndex],
      });
    });
    drake.on('drag', (el: any, source: any) => {
      if (!drake.models) {
        return;
      }
      dragElm = el;
      dragIndex = this.domIndexOf(el, source);
    });
    drake.on(
      'drop',
      (dropElm: any, target: Element, source: Element, sibling?: Element) => {
        if (!drake.models || !target) {
          return;
        }
        dropIndex = this.domIndexOf(dropElm, target);
        let sourceModel = drake.models[drake.containers.indexOf(source)];
        let targetModel = drake.models[drake.containers.indexOf(target)];
        // console.log('DROP');
        // console.log(sourceModel);
        let item: any;
        if (target === source) {
          sourceModel = sourceModel.slice(0);
          item = sourceModel.splice(dragIndex, 1)[0];
          sourceModel.splice(dropIndex, 0, item);
          // this was true before we cloned and updated sourceModel,
          // but targetModel still has the old value
          targetModel = sourceModel;
        } else {
          const isCopying = dragElm !== dropElm;
          item = sourceModel[dragIndex];
          if (isCopying) {
            if (!options.copyItem) {
              throw new Error(
                'If you have enabled `copy` on a group, you must provide a `copyItem` function.'
              );
            }
            item = options.copyItem(item);
          }

          if (!isCopying) {
            sourceModel = sourceModel.slice(0);
            sourceModel.splice(dragIndex, 1);
          }
          targetModel = targetModel.slice(0);
          targetModel.splice(dropIndex, 0, item);
          if (isCopying) {
            try {
              target.removeChild(dropElm);
            } catch (e) { }
          }
        }
        this.dispatch$.next({
          event: EventTypes.DropModel,
          name,
          args: [
            dropElm,
            target,
            source,
            sibling,
            item,
            sourceModel,
            targetModel,
            dragIndex,
            dropIndex,
          ],
        });
      }
    );
  }

  private setupEvents(group: Group): void {
    if (group.initEvents) {
      return;
    }
    group.initEvents = true;
    const name = group.name;
    const that: any = this;
    const emitter = (event: EventTypes) => {
      group.drake.on(event, (...args: any[]) => {
        this.dispatch$.next({ event, name, args });
      });
    };
    AllEvents.forEach(emitter);
  }

  private domIndexOf(child: any, parent: any): any {
    const tempVal = Array.prototype.indexOf.call(parent.children, child);
    return tempVal;
  }
}

export class SubDragulaService {

  private dispatch$ = new Subject<Dispatch>();
  elContainerSource = (eventType: EventTypes) => (groupName?: string) =>
    this.dispatch$.pipe(
      filterEvent(eventType, groupName, elContainerSourceProjector)
    )
}
