import { User, RoleDetails } from '@mts-appsession-model';
import { JwtHelperService } from '@auth0/angular-jwt';
import { isTruthy } from '@mts-functions/is-truthy.function';
import { environment } from 'src/environments/environment';
import { AppSettings } from '../../constant/app-setting-constants/app-setting.constant';

const jwtHelper = new JwtHelperService();
export class SessionHelper {

  static setSessionVariables() {
    if (!isTruthy(this.UserDetails) && localStorage.getItem('userDetails') !== null) {
      this.UserDetails = jwtHelper.decodeToken(localStorage.getItem('userDetails'))['data'].User;
      AppSettings.SessionErrorMsg = true;
    }
    if (!isTruthy(this.RoleDetails) && localStorage.getItem('roleDetails') !== null) {
      if (environment.ADAuthentication) {
        this.RoleDetails = jwtHelper.decodeToken(localStorage.getItem('roleDetails'))['data'];
      } else {
        this.RoleDetails = jwtHelper.decodeToken(localStorage.getItem('roleDetails'))['data'];

      }
    }
  }

  static setUserSessionVariables() {
    if (!isTruthy(this.UserDetails) && localStorage.getItem('userDetails') !== null) {
      this.UserDetails = jwtHelper.decodeToken(localStorage.getItem('userDetails'))['data'].User;
      AppSettings.SessionErrorMsg = true;
    }
  }
  static cleanSessionVariables() {
    localStorage.removeItem('id_token');
    localStorage.removeItem('roleDetails');
    localStorage.removeItem('userDetails');

    this.UserDetails = null;
    this.RoleDetails = null;
    AppSettings.SessionErrorMsg = false;
  }
  static UserDetails: User;
  static RoleDetails: RoleDetails;
  showSideBar = false;
}
