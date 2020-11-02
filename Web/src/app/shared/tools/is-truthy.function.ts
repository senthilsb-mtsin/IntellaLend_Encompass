export function isTruthy(data: any): boolean {
  if (data !== null && data !== undefined) {
    return typeof data === 'string' && data.trim().length < 1 ? false : true;
  }

  return false;
}
