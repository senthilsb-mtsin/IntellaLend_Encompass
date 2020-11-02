export class SelectItem {
  id: string;
  text: string;
  children: SelectItem[];
  parent: SelectItem;

  constructor(source: any) {
    if (typeof source === 'string') {
      this.id = this.text = source;
    }
    if (typeof source === 'object') {
      this.id = source.id || source.text;
      this.text = source.text;
      if (source.children && source.text) {
        this.children = source.children.map((c: any) => {
          const r: SelectItem = new SelectItem(c);
          r.parent = this;
          return r;
        });
        this.text = source.text;
      }
    }
  }

  fillChildrenHash(optionsMap: Map<string, number>, startIndex: number): number {
    let i = startIndex;
    this.children.map((child: SelectItem) => {
      optionsMap.set(child.id, i++);
    });
    return i;
  }

  hasChildren(): boolean {
    return this.children && this.children.length > 0;
  }

  getSimilar(): SelectItem {
    const r: SelectItem = new SelectItem(false);
    r.id = this.id;
    r.text = this.text;
    r.parent = this.parent;
    return r;
  }
}
