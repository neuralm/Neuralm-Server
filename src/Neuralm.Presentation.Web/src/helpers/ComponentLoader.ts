/**
 * Loads the component by path.
 * @param path The component path.
 */
export default function ComponentLoader(path: string): any {
  return import(`../components/${path}.vue`);
}
