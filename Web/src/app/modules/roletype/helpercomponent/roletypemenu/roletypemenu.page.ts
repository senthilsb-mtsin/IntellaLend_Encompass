import { NotificationService } from './../../../../shared/service/notification.service';
import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { AppSettings } from '@mts-app-setting';
import { RoleTypeService } from '../../service/roletype.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';
import { TrimspacePipe } from '@mts-pipe';
import { Roletypemodel } from '../../models/roletype-datatable.model';
import { ChangeRoleRequest } from '../../models/changerole.model';

@Component({
  selector: 'mts-roletype-menu',
  templateUrl: './roletypemenu.page.html',
  styleUrls: ['./roletypemenu.page.css'],
})
export class RoleTypeMenuComponent implements OnInit, OnDestroy {
  menulist: any;
  isSubMenu = 'none';
  ismapped = false;
  _setHomePage: any = [];
  _viewsubmenu = false;
  roletypedata: Roletypemodel;
  constructor(private _roleTypeService: RoleTypeService, private _route: Router, private trim: TrimspacePipe, private _notificationservice: NotificationService) { }
  private subscription: Subscription[] = [];
  ngOnInit() {

    this.subscription.push(
      this._roleTypeService.menulist$.subscribe((res: any) => {
        this.menulist = res;
      })
    );
    this.subscription.push(
      this._roleTypeService.RoleTypeRowData.subscribe(
        (res: Roletypemodel) => {
          this.roletypedata = res;
          this.ismapped = this.roletypedata.RoleID === -1 ? false : true;
        })
    );
  }
  StartPage(event: any, menuid: any) {
    for (let x = 0; x < this.menulist.length; x++) {
      if (this.menulist[x].MenuGroupID > 0) {
        for (let m = 0; m < this.menulist[x].SubMenus.length; m++) {
          if (this.menulist[x].SubMenus[m].MenuID === menuid) {
            this._setHomePage.push({
              IsChecked: this.menulist[x].SubMenus[m].IsChecked
            });
            if (this.ismapped) {
            this.menulist[x].SubMenus[m].IsMapped = true;
              this.MenuActive(this.menulist[x].SubMenus[m]);
            } else {
              this.menulist[x].SubMenus[m].Active = true;
              this.MenuActive(this.menulist[x].SubMenus[m]);
            }

          } else {
            this.menulist[x].SubMenus[m].IsChecked = false;
          }
        }
      } else {
        for (let m = 0; m < this.menulist[x].SubMenus.length; m++) {
          if (this.menulist[x].SubMenus[m].MenuID === menuid) {
            this._setHomePage.push({
              IsChecked: this.menulist[x].SubMenus[m].IsChecked
            });
            if (this.ismapped) {
              this.menulist[x].SubMenus[m].IsMapped = true;
            } else {
              this.menulist[x].SubMenus[m].Active = true;
            }
          } else {
            this.menulist[x].SubMenus[m].IsChecked = false;
          }
        }
      }
    }
    this._roleTypeService.menulist$.next(this.menulist);
  }

  ShowSubMenus() {
    if (this.isSubMenu === 'none') {
      this.isSubMenu = 'block';
      this._viewsubmenu = true;
    } else {
      this.isSubMenu = 'none';
      this._viewsubmenu = false;
    }

  }
  MenuActive(CurrObj: any) {
    if (CurrObj.SubMenuID.length > 0) {
      this.menulist.forEach(menuGrp => {
        menuGrp.SubMenus.forEach(menu => {
          CurrObj.SubMenuID.forEach(subMenuID => {
            if (menu.MenuID === subMenuID) {
              if (this.ismapped) {
                menu.IsMapped = false;
              } else {
                menu.Active = false;
              }
              if (typeof menu['IsChecked'] !== 'undefined' && menu['IsChecked'] === true) {
                menu['IsChecked'] = false;
                CurrObj['IsChecked'] = true;
              }
            }
          });
        });
      });
    }
    if (this.ismapped) {
      if (CurrObj.IsMapped === false) {CurrObj.IsChecked = false; }
    } else {
      if (CurrObj.Active === false) { CurrObj.IsChecked = false; }
    }
    //
    this.ChangeMenuActive(CurrObj);
    this._roleTypeService.menulist$.next(this.menulist);
  }
  ngOnDestroy() {
    this.subscription.forEach(element => {
      element.unsubscribe();
    });
  }
  ChangeMenuActive(menudetail: any) {
    if (this.roletypedata.RoleID) {
      const ChangeMenu = new ChangeRoleRequest(
        AppSettings.TenantSchema, this.roletypedata.RoleID, menudetail);
      this._roleTypeService.GetChangeMenuActive(ChangeMenu);
    }
  }
}
