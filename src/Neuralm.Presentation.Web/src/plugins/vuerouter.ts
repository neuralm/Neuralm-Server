import Vue from 'vue';
import Router, { RouteConfig, RawLocation, Route } from 'vue-router';

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
   * @param routes The routes the router is allowed to go to.
   * @param addRedirect Whether to add a redirect on unknown routes to the first route in the routes array.
   */
  constructor(routes: NeuralmRoute[], addRedirect: boolean) {
    super({ mode: 'history' });

    // redirect to login page if not logged in and trying to access a restricted page.
    this.beforeEach((to, _, next) => {
      const publicPages = ['/login', '/register'];
      const authRequired = !publicPages.includes(to.path);
      const loggedIn = localStorage.getItem('user');
      if (authRequired && !loggedIn) {
        return next('/login');
      }
      next();
    });

    // set the routes.
    const actualRoutes: RouteConfig[] = new Array();
    routes.forEach((route) => {
      const view: string = route.name.replace(/^\w/, (c) => c.toUpperCase());
      const routeConfig: RouteConfig = { path: `/${route.name}`, name: route.name, component: () => NeuralmRouter.loadView(view), props: route.props };
      actualRoutes.push(routeConfig);
    });
    if (addRedirect && actualRoutes.length > 0) {
      actualRoutes.push({ path: '*', redirect: actualRoutes[0].path });
    }
    super.addRoutes(actualRoutes);
  }

  /**
   * Pushes the location to the router stack.
   * Overridden to ignore NavigationDuplicated error.
   * @param location The raw location.
   * @returns Returns the promised route.
   */
  public async push(location: RawLocation): Promise<Route> {
    try {
      return super.push(location);
    }
    catch (err) {
      if (err === undefined) {
        return Promise.resolve((undefined as any));
      }
      else if (err.name !== 'NavigationDuplicated') {
        return Promise.reject(err);
      }
      else {
        return Promise.resolve(({ message: 'Router failsafe triggered' } as any));
      }
    }
  }

  /* tslint:disable */
  private static loadView = (name: string): any => import(`../views/${name}.vue`);
  /* tslint:enable */
}
