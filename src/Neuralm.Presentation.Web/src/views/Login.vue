<template>
  <div>
    <h2>Login</h2>
    <form @submit.prevent="handleSubmit">
      <div class="form-group">
        <label for="username">Username</label>
        <input type="text" v-model="username" name="username" class="form-control" :class="{ 'is-invalid': submitted && !username }" />
        <div v-show="submitted && !username" class="invalid-feedback">Username is required</div>
      </div>
      <div class="form-group">
        <label htmlFor="password">Password</label>
        <input type="password" v-model="password" name="password" class="form-control" :class="{ 'is-invalid': submitted && !password }" />
        <div v-show="submitted && !password" class="invalid-feedback">Password is required</div>
      </div>
      <div class="form-group">
        <button class="btn btn-primary" :disabled="status.loggingIn">Login</button>
        <img v-show="status.loggingIn" src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA==" />
        <router-link to="/register" class="btn btn-link">Register</router-link>
      </div>
    </form>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { mapState, mapActions } from 'vuex';
import IUserService from '../interfaces/IUserService';
import User from '../models/user';
import AuthenticateRequest from '../messages/requests/AuthenticateRequest';
import AuthenticateResponse from '../messages/responses/AuthenticateResponse';

@Component({
  data: () => ({
    username: '',
    password: '',
    submitted: false
  }),
  computed: {
    ...mapState('user', {
      status: 'status'
    })
  }
})
export default class LoginView extends Vue {
  @Prop() private userService!: IUserService;

  public login(username: string, password: string): void {
    this.$store.commit('user/loginRequest', { username });
    this.userService.login(new AuthenticateRequest(username, password))
      .then(
        (response: AuthenticateResponse) => {
          console.log('LOGIN SUCCESS');
          const user: User = { userId: response.UserId, accessToken: response.AccessToken };
          this.$store.commit('user/loginSuccess', user);
          this.$router.push('/');
        },
        (error: Promise<any>) => {
          console.log(error);
          console.log('LOGIN FAILURE');
          this.$store.commit('user/loginFailure');
          this.$store.dispatch('alert/error', error, { root: true });
        }
      );
  }

  public logout() {
    this.userService.logout();
    this.$store.commit('user/logout');
  }

  public handleSubmit(e: Event): void {
    this.$data.submitted = true;
    const { username, password } = this.$data;
    if (username && password) {
      this.login(username, password);
    }
  }
}
</script>