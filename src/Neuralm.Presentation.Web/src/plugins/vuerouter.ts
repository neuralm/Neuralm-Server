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
   */
  constructor() {
    super({ mode: 'history' });
    this.beforeEach((to, _, next) => {
      // redirect to login page if not logged in and trying to access a restricted page.
      // console.log(`Routed to ${to.name}`);
      const publicPages = ['/login', '/register'];
      const authRequired = !publicPages.includes(to.path);
      const loggedIn = localStorage.getItem('user');
      if (authRequired && !loggedIn) {
        // console.log('redirected to login!');
        return next('/login');
      }
      next();
    });
  }

  public push(location: RawLocation): Promise<Route> {
    return super.push(location).catch((err) => {
      if (err.name !== 'NavigationDuplicated') {
        throw err;
      }
      return Promise.resolve(undefined as any);
    });
  }

  public setRoutes(routes: NeuralmRoute[], addRedirect: boolean): void {
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

  /* tslint:disable */
  private static loadView = (name: string): any => import(`../views/${name}.vue`);
  /* tslint:enable */
}
