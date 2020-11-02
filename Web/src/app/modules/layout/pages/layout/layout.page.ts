import { Component, AfterViewInit } from '@angular/core';
import {
  trigger,
  state,
  style,
  transition,
  animate,
} from '@angular/animations';
import { LayoutService } from '../../service/layout.service';
import { SessionHelper } from '@mts-app-session';
import { isTruthy } from '@mts-functions/is-truthy.function';

@Component({
  moduleId: 'LayoutComponent',
  selector: 'mts-layout',
  templateUrl: 'layout.page.html',
  styleUrls: ['layout.page.css'],
  animations: [
    trigger('slideInOut', [
      state(
        'in',
        style({
          transform: 'translate3d(-100%, 0, 0)',
        })
      ),
      state(
        'out',
        style({
          transform: 'translate3d(0, 0, 0)',
        })
      ),
      transition('in => out', animate('200ms ease-in-out')),
      transition('out => in', animate('200ms ease-in-out')),
    ]),
  ],
})
export class LayoutComponent implements AfterViewInit {
  showSideBar = true;
  menus: any;
  activeURL: any;
  viewFotterClass = 'page-content-wrapper';
  mainViewClass = 'page-container';
  menuColors: any = [
    'txt-indigo',
    'txt-orange',
    'txt-green',
    'txt-themeRed',
    'txt-warm',
    'txt-info',
    'txt-brown',
    'txt-green',
  ];
  constructor(private _layoutService: LayoutService) {
    this.setMenu();
  }

  ngAfterViewInit() {
    this.setMenusArray();
  }

  setMenu() {
    this.menus = SessionHelper.RoleDetails.Menus;
    for (let i = 0; i < this.menus.length; i++) {
      if (this.menus[i].MenuGroupID > 0) {
        this.menus[i]['color'] = this.menuColors[i % 7];
      }

      for (let x = 0; x < this.menus[i].SubMenus.length; x++) {
        this.menus[i].SubMenus[x]['color'] = this.menuColors[x % 7];
      }
    }
  }

  setMenusArray() {
    if (isTruthy(SessionHelper.UserDetails)) {
      const userRoleMapping = SessionHelper.UserDetails.UserRoleMapping;
      if (userRoleMapping.length > 1) {
        this._layoutService.ischangeRole.next(true);
      } else {
        this._layoutService.ischangeRole.next(false);
      }
    }
  }
}
