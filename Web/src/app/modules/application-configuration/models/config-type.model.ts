export class ConfigTypeModel {
  ConfigKey: string;
  ConfigValue: string;
  ConfigType: string;

  constructor(ConfigKey: string, ConfigValue: string, ConfigType: string) {
    this.ConfigKey = ConfigKey;
    this.ConfigValue = ConfigValue;
    this.ConfigType = ConfigType;
  }
}
