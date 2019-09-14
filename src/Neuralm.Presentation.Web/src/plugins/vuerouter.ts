import Vue from 'vue';
import Router, { RouteConfig } from 'vue-router';

Vue.use(Router);

/**
 * Represents the NeuralmRoute interface.
 */
export interface NeuralmRoute {
  name: string;
  props?: object;
}

/**
 * Represents the NeuralmRouter class.
 */
export default class NeuralmRouter extends Router {

  /**
   * Initializes an instance of the NeuralmRouter class.
   */
  constructor() {
    super({ mode: 'history' });
  }

  public setRoutes(routes: NeuralmRoute[], addRedirect: boolean): void {
    const actualRoutes: RouteConfig[] = new Array();
    routes.forEach((route) => {
      const view: string = route.name.replace(/^\w/, (c) => c.toUpperCase());
      console.log(view);
      const routeConfig: RouteConfig = { path: `/${route.name}`, name: route.name, component: () => NeuralmRouter.loadView(view), props: route.props };
      actualRoutes.push(routeConfig);
    });
    console.log(actualRoutes);
    if (addRedirect && actualRoutes.length > 0) {
      actualRoutes.push({ path: '*', redirect: actualRoutes[0].path });
    }
    super.addRoutes(actualRoutes);
  }

  /* tslint:disable */
  private static loadView = (name: string): any => import(`../views/${name}.vue`);
  /* tslint:enable */
}
