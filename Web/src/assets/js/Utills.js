String.prototype.parseBoolean = function() {
    // import { element } from 'protractor';
    
    if (this.toString() != null || this.toString() != "" || this.toString() != 'undefined') {
        switch (this.toString()) {
            case 'true':
            case 'yes':
            case '1':
                return true;
            case 'false':
            case 'no':
            case '0':
                return false;
            default:
                return Boolean(this.toString());
        }
    }
}

String.prototype.isNullorEmpty = function() {
    
    return (!this || this == undefined || this == "" || this.length == 0);
}