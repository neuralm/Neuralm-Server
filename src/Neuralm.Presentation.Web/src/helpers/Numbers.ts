/**
 * Ensures that numbers on interfaces are actually numbers.
 * @param obj The object.
 * @returns The ensured object.
 */
function ensureNumbers<T>(obj: T): T {
  for (const [key, value] of Object.entries(obj)) {
      if (isNumber(value)) {
      Object.assign(obj, { [key]: parseFloat(String(value)) });
      }
  }
  return obj;
}

/**
 * Checks if the given value is a number.
 * @param value The number.
 * @returns Returns whether the value is a number.
 */
function isNumber(value: string | number): boolean {
  return !isNaN(parseFloat(String(value))) && isFinite(Number(value));
}

export { ensureNumbers, isNumber };
