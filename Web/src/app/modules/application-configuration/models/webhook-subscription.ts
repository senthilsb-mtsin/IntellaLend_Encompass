/**
 * Api parameter Model to check `WebHook Subscription Event type` Exists
 */
export class CheckWebHookEventTypeExistModel {
    /**
     * Create new `CheckWebHookEventTypeExistModel` object
     * @param TableSchema TableSchema
     * @param EventType Event type number
     */
    constructor(public TableSchema: string,
        public EventType: number) {
    }
}
/**
 * Api parameter Model to `create` `WebHook Subscription Event type`
 */
export class CreateWebHookEventTypeModel {
    /**
     * 
     * @param TableSchema TableSchema
     * @param WebHookType Event type number
     */
    constructor(public TableSchema: string,
        public WebHookType: number) { }
}
/**
 * Api parameter Model to `delete` `WebHook Subscription Event type`
 */
export class DeleteWebHookEventTypeModel {
    /**
     * 
     * @param TableSchema TableSchema
     * @param WebHookType Event type number
     */
    constructor(public TableSchema: string,
        public WebHookType: number) { }
}
