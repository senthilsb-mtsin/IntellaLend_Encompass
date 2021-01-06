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
