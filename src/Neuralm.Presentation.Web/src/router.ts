import Vue from 'vue';
import Router, { Route } from 'vue-router';
import Home from './views/Home.vue';
import About from './views/About.vue';

Vue.use(Router);

export const router: Router = new Router({
  mode: 'history',
  routes: [
    { path: '/', name: 'home', component: Home },
    { path: '/about', name: 'about', component: About },

    // otherwise redirect to home.
    { path: '*', redirect: '/' }
  ]
});

router.beforeEach((to: Route, from: Route, next) => {
  // redirect to login page if not logged in and trying to access a restricted page.
  const publicPages = ['/login', '/register'];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem('user');

  if (authRequired && !loggedIn) {
    return next('/login');
  }

  next();
});

