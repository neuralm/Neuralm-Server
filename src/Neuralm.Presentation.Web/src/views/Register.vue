<template>
  <div>
    <h2>Register</h2>
    <form @submit.prevent="handleSubmit">
      <div class="form-group">
        <label for="username">Username</label>
        <input type="text" v-model="username" name="username" class="form-control" :class="{ 'is-invalid': submitted && username.length > 3 }" />
      </div>
      <div class="form-group">
        <label htmlFor="password">Password</label>
        <input type="password" v-model="password" name="password" class="form-control" :class="{ 'is-invalid': submitted && password.length > 3 }" />
      </div>
      <div class="form-group">
        <button class="btn btn-primary" :disabled="status.registering">Register</button>
        <img
          v-show="status.registering"
          src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA=="
        />
        <router-link to="/login" class="btn btn-link">Cancel</router-link>
      </div>
    </form>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Inject } from 'vue-property-decorator';
import { mapState, mapActions } from 'vuex';
import IUserService from '../interfaces/IUserService';
import User from '../models/user';
import RegisterRequest from '../messages/requests/RegisterRequest';
import RegisterResponse from '../messages/responses/RegisterResponse';

@Component({
  data: () => ({
    username: '',
    password: '',
    submitted: false
  }),
  computed: {
    ...mapState('user', ['status'])
  }
})
export default class RegisterView extends Vue {
  @Prop() private userService!: IUserService;

  public register(username: string, password: string): void {
    this.$store.commit('user/registerRequest');
    this.userService.register(new RegisterRequest(username, password)).then(
      (response: RegisterResponse) => {
        this.$store.commit('user/registerSuccess');
        if (response.success) {
          this.$snotify.success('Registration successful!');
          this.$router.push('/login');
        } else {
          this.$store.commit('user/registerFailure');
          this.$snotify.success('Registration failed!');
        }
      },
      (error: Promise<RegisterResponse>) => {
        this.$store.commit('user/registerFailure');
        error.then((value: RegisterResponse) => {
          this.$snotify.error(value.message);
        });
      }
    );
  }

  public handleSubmit(e: Event) {
    this.$data.submitted = true;
    // TODO: add proper validation.
    const valid: boolean = true;
    if (valid) {
      this.register(this.$data.username, this.$data.password);
    }
  }
}
</script>
