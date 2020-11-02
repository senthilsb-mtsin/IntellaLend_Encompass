import { User, RoleDetails } from '@mts-appsession-model';
import { JwtHelperService } from '@auth0/angular-jwt';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { environment } from 'src/environments/environment';

const jwtHelper = new JwtHelperService();
export class SessionHelper {

  static setSessionVariables() {
    if (!isTruthy(this.UserDetails) && localStorage.getItem('userDetails') !== null) {
      this.UserDetails = jwtHelper.decodeToken(localStorage.getItem('userDetails'))['data'].User;
    }
    if (!isTruthy(this.RoleDetails) && localStorage.getItem('roleDetails') !== null) {
      if (environment.ADAuthentication) {
        this.RoleDetails = jwtHelper.decodeToken(localStorage.getItem('roleDetails'))['data'];
        for (let j = 0; j < this.RoleDetails.Menus[1].SubMenus.length; j++) {

          if (this.RoleDetails.Menus[1].SubMenus[j].MenuTitle === 'User Administration') {
            this.RoleDetails.Menus[1].SubMenus.splice(j, 1);
          }

        }
      } else {
        this.RoleDetails = jwtHelper.decodeToken(localStorage.getItem('roleDetails'))['data'];

      }
    }
  }

  static cleanSessionVariables() {
    localStorage.removeItem('id_token');
    localStorage.removeItem('roleDetails');
    localStorage.removeItem('userDetails');

    this.UserDetails = null;
    this.RoleDetails = null;
  }
  static UserDetails: User;
  static RoleDetails: RoleDetails;
  showSideBar = false;
}
