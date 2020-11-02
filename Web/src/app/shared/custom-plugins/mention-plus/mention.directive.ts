import { Input, EventEmitter, Output, OnChanges, SimpleChanges, Directive, ElementRef, ComponentFactoryResolver, ViewContainerRef, TemplateRef } from '@angular/core';

import { MentionConfig } from './mention-config';
import { MentionListComponent } from './mention-list.component';
import { getValue, insertValue, getCaretPosition, setCaretPosition } from './mention-utils';

const KEY_BACKSPACE = 8;
const KEY_TAB = 9;
const KEY_ENTER = 13;
const KEY_SHIFT = 16;
const KEY_ESCAPE = 27;
const KEY_SPACE = 32;
const KEY_LEFT = 37;
const KEY_UP = 38;
const KEY_RIGHT = 39;
const KEY_DOWN = 40; const KEY_2 = 50;

/**
 * Angular 2 Mentions.
 * https://github.com/dmacfarlane/angular-mentions
 *
 * Copyright (c) 2017 Dan MacFarlane
 */
@Directive({
  selector: '[mention], [mentionConfig]',
  host: {
    '(keydown)': 'keyHandler($event)',
    '(blur)': 'blurHandler($event)',
    'autocomplete': 'off'
  }
})
export class MentionDirective implements OnChanges {

  @Input('mention') set mention(items: any[]) {
    this.mentionItems = items;
  }

  // the provided configuration object
  @Input() mentionConfig: MentionConfig = { items: [] };

  // template to use for rendering list items
  @Input() mentionListTemplate: TemplateRef<any>;

  // event emitted whenever the search term changes
  @Output() searchTerm = new EventEmitter();

  // Prakash: Emit Selected Item
  @Output() selectedItem = new EventEmitter();

  @Input() originalFieldArry: any;
  // private originalFieldArry: any = [];

  constructor(
    private _element: ElementRef,
    private _componentResolver: ComponentFactoryResolver,
    private _viewContainerRef: ViewContainerRef
  ) { }

  // stores the items passed to the mentions directive and used to populate the root items in mentionConfig
  private mentionItems: any[];

  private activeConfig: MentionConfig;

  private DEFAULT_CONFIG: MentionConfig = {
    items: [],
    triggerChar: '@',
    labelKey: 'label',
    maxItems: -1,
    mentionSelect: (item: any) => this.activeConfig.triggerChar + item[this.activeConfig.labelKey]
  };

  private triggerChars: { [key: string]: MentionConfig } = {};

  private searchString: string;
  private startPos: number;
  private startNode;
  private searchList: MentionListComponent;
  private searching: boolean;
  private iframe: any; // optional
  private previousValue: any;

  ngOnChanges(changes: SimpleChanges) {

    // console.log('config change', changes);
    if (changes['mention'] || changes['mentionConfig']) {
      this.updateConfig();
    }
  }

  setIframe(iframe: HTMLIFrameElement) {
    this.iframe = iframe;
  }

  stopEvent(event: any) {
    // if (event instanceof KeyboardEvent) { // does not work for iframe
    if (!event.wasClick) {
      event.preventDefault();
      event.stopPropagation();
      event.stopImmediatePropagation();
    }
  }

  blurHandler(event: any) {
    this.stopEvent(event);
    this.stopSearch();
  }

  // @param nativeElement is the alternative text element in an iframe scenario
  keyHandler(event: any, nativeElement: HTMLInputElement = this._element.nativeElement) {

    // if (!event.shiftKey)

    const val: string = getValue(nativeElement);
    let pos = getCaretPosition(nativeElement, this.iframe);
    let charPressed = event.key;
    if (!charPressed) {
      const charCode = event.which || event.keyCode;
      if (!event.shiftKey && (charCode >= 65 && charCode <= 90)) {
        charPressed = String.fromCharCode(charCode + 32);
      } else {
        // TODO (dmacfarlane) fix this for non-alpha keys
        // http://stackoverflow.com/questions/2220196/how-to-decode-character-pressed-from-jquerys-keydowns-event-handler?lq=1
        charPressed = String.fromCharCode(event.which || event.keyCode);
      }
    }
    if (event.keyCode === KEY_ENTER && event.wasClick && pos < this.startPos) {
      // put caret back in position prior to contenteditable menu click
      pos = this.startNode.length;
      setCaretPosition(this.startNode, pos, this.iframe);
    }
    // console.log("keyHandler", this.startPos, pos, val, charPressed, event);

    const config = this.triggerChars[charPressed];
    if (config) {
      if (charPressed === '.') {

        config.items = [];
        const fieldItems = this.originalFieldArry.filter((x: any) => x.DocID === this.previousValue);
        config.labelKey = 'label';
        fieldItems.forEach(element => {
          config.items.push({ label: element.DocFieldName });
        });

        this.addConfig(config);
      }
      this.activeConfig = config;
      this.startPos = pos;
      this.startNode = (this.iframe ? this.iframe.contentWindow.getSelection() : window.getSelection()).anchorNode;
      this.searching = true;
      this.searchString = null;
      this.showSearchList(nativeElement);
      this.updateSearchList();
      this.previousValue = this.previousValue;
    } else if (this.startPos >= 0 && this.searching) {
      if (pos <= this.startPos) {
        this.searchList.hidden = true;
        this.previousValue = this.previousValue;
      } else if (event.keyCode !== KEY_SHIFT &&
        !event.metaKey &&
        !event.altKey &&
        !event.ctrlKey &&
        pos > this.startPos
      ) {
        if (event.keyCode === KEY_SPACE) {
          this.startPos = -1;
          this.previousValue = this.previousValue;
        } else if (event.keyCode === KEY_BACKSPACE && pos > 0) {
          pos--;
          if (pos === this.startPos) {
            this.previousValue = this.previousValue;
            this.stopSearch();
          }
        } else if (!this.searchList.hidden) {
          if (event.keyCode === KEY_TAB || event.keyCode === KEY_ENTER) {
            this.stopEvent(event);
            // const text = this.activeConfig.mentionSelect(this.searchList.activeItem);

            let text = '';
            if (this.activeConfig.triggerChar === '#' || this.activeConfig.triggerChar === '.') {
              text = this.activeConfig.triggerChar === '#' ? '{' + this.searchList.activeItem.label : this.activeConfig.triggerChar + this.searchList.activeItem.label + '}';
            }

            if (this.activeConfig.triggerChar === '$') {
              text = '[' + this.searchList.activeItem.label + ']';
            }

            if (this.activeConfig.triggerChar !== '#' && this.activeConfig.triggerChar !== '$' && this.activeConfig.triggerChar !== '.') {
              text = this.activeConfig.triggerChar + this.searchList.activeItem.label;
            }

            // value is inserted without a trailing space for consistency
            // between element types (div and iframe do not preserve the space)
            insertValue(nativeElement, this.startPos, pos, text, this.iframe);
            // fire input event so angular bindings are updated
            if ('createEvent' in document) {
              const evt = document.createEvent('HTMLEvents');
              if (this.iframe) {
                // a 'change' event is required to trigger tinymce updates
                evt.initEvent('change', true, false);
              } else {
                evt.initEvent('input', true, false);
              }
              // this seems backwards, but fire the event from this elements nativeElement (not the
              // one provided that may be in an iframe, as it won't be propogate)
              this._element.nativeElement.dispatchEvent(evt);
            }
            this.startPos = -1;

            // this.selectedItem.emit(this.searchList.activeItem.label);
            if (charPressed !== '.') {
              this.previousValue = this.searchList.activeItem.label;
            }

            this.stopSearch();
            return false;
          } else if (event.keyCode === KEY_ESCAPE) {
            this.previousValue = this.previousValue;
            this.stopEvent(event);
            this.stopSearch();
            return false;
          } else if (event.keyCode === KEY_DOWN) {
            this.previousValue = this.previousValue;
            this.stopEvent(event);
            this.searchList.activateNextItem();
            return false;
          } else if (event.keyCode === KEY_UP) {
            this.previousValue = this.previousValue;
            this.stopEvent(event);
            this.searchList.activatePreviousItem();
            return false;
          }
        }

        if (event.keyCode === KEY_LEFT || event.keyCode === KEY_RIGHT) {
          this.previousValue = this.previousValue;
          this.stopEvent(event);
          return false;
        } else if (this.searching) {
          let mention = val.substring(this.startPos + 1, pos);
          this.previousValue = this.previousValue;
          if (event.keyCode !== KEY_BACKSPACE) {
            mention += charPressed;
          }
          this.searchString = mention;
          this.searchTerm.emit(this.searchString);
          this.updateSearchList();
        }
      }
    }
  }

  stopSearch() {
    if (this.searchList) {
      this.searchList.hidden = true;
    }
    this.activeConfig = null;
    this.searching = false;
  }

  updateSearchList() {
    let matches: any[] = [];
    if (this.activeConfig && this.activeConfig.items) {
      let objects = this.activeConfig.items;
      // disabling the search relies on the async operation to do the filtering
      if (!this.activeConfig.disableSearch && this.searchString) {
        const searchStringLowerCase = this.searchString.toLowerCase();
        objects = objects.filter(e => e[this.activeConfig.labelKey].toLowerCase().startsWith(searchStringLowerCase));
      }
      matches = objects;
      if (this.activeConfig.maxItems > 0) {
        matches = matches.slice(0, this.activeConfig.maxItems);
      }
    }
    // update the search list
    if (this.searchList) {
      this.searchList.labelKey = this.activeConfig.labelKey;
      this.searchList.items = matches;
      this.searchList.hidden = matches.length === 0;
    }
  }

  showSearchList(nativeElement: HTMLInputElement) {
    if (this.searchList === undefined || this.searchList === null) {
      const componentFactory = this._componentResolver.resolveComponentFactory(MentionListComponent);
      const componentRef = this._viewContainerRef.createComponent(componentFactory);
      this.searchList = componentRef.instance;
      this.searchList.position(nativeElement, this.iframe, this.activeConfig.dropUp);
      this.searchList.itemTemplate = this.mentionListTemplate;
      componentRef.instance['itemClick'].subscribe(() => {
        nativeElement.focus();
        const fakeKeydown = { 'keyCode': KEY_ENTER, 'wasClick': true };
        this.keyHandler(fakeKeydown, nativeElement);
      });
    } else {
      this.searchList.labelKey = this.activeConfig.labelKey;
      this.searchList.activeIndex = 0;
      this.searchList.position(nativeElement, this.iframe, this.activeConfig.dropUp);
      window.setTimeout(() => this.searchList.resetScroll());
    }
  }

  private updateConfig() {

    const config = this.mentionConfig;
    this.triggerChars = {};
    // use items from directive if they have been set
    if (this.mentionItems) {
      config.items = this.mentionItems;
    }
    this.addConfig(config);
    // nested configs
    if (config !== undefined) {
      if (config.mentions) {
        config.mentions.forEach(con => this.addConfig(con));
      }
    }
  }

  // add configuration for a trigger char
  private addConfig(config: MentionConfig) {

    const defaults = Object.assign({}, this.DEFAULT_CONFIG);
    config = Object.assign(defaults, config);
    // items
    let items = config.items;
    if (items && items.length > 0) {
      // convert strings to objects
      if (typeof items[0] === 'string') {
        items = items.map((label) => {
          const object = {};
          object[config.labelKey] = label;
          return object;
        });
      }
      // remove items without an labelKey (as it's required to filter the list)
      items = items.filter(e => e[config.labelKey]);
      if (!config.disableSort) {
        items.sort((a, b) => a[config.labelKey].localeCompare(b[config.labelKey]));
      }
    }
    config.items = items;

    // add the config
    this.triggerChars[config.triggerChar] = config;

    // for async update while menu/search is active
    if (this.activeConfig && this.activeConfig.triggerChar === config.triggerChar) {
      this.activeConfig = config;
      this.updateSearchList();
    }
  }
}
