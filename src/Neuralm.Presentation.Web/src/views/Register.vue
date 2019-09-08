<template>
    <div>
        <h2>Register</h2>
        <form @submit.prevent="handleSubmit">
            <div class="form-group">
                <label for="firstName">First Name</label>
                <input type="text" v-model="user.firstName" name="firstName" class="form-control" :class="{ 'is-invalid': submitted && user.firstName.length > 3 }" />
            </div>
            <div class="form-group">
                <label for="lastName">Last Name</label>
                <input type="text" v-model="user.lastName" name="lastName" class="form-control" :class="{ 'is-invalid': submitted && user.lastName.length > 3 }" />
            </div>
            <div class="form-group">
                <label for="username">Username</label>
                <input type="text" v-model="user.username" name="username" class="form-control" :class="{ 'is-invalid': submitted && user.username.length > 3 }" />
            </div>
            <div class="form-group">
                <label htmlFor="password">Password</label>
                <input type="password" v-model="user.password" name="password" class="form-control" :class="{ 'is-invalid': submitted && user.password.length > 3 }" />
            </div>
            <div class="form-group">
                <button class="btn btn-primary" :disabled="status.registering">Register</button>
                <img v-show="status.registering" src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA==" />
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

@Component({
  data: () => ({
    user: {
      firstName: '',
      lastName: '',
      username: '',
      password: ''
    },
    submitted: false
  }),
  computed: {
    ...mapState('user', {
      status: 'status'
    })
  }
})
export default class RegisterView extends Vue {
  @Prop() private userService!: IUserService;

  public register(user: User): void {
    this.$store.commit('user/registerRequest', user);
    this.userService.register(user)
      .then(
        (success: boolean) => {
          this.$store.commit('user/registerSuccess', success);
          this.$router.push('/login');
          setTimeout(() => {
            this.$store.dispatch('alert/success', 'Registration successful', { root: true });
          });
        },
        (error: Promise<any>) => {
          this.$store.commit('user/registerFailure', error);
          this.$store.dispatch('alert/error', error, { root: true });
        }
      );
  }

  public handleSubmit(e: Event) {
    this.$data.submitted = true;
    // TODO: add proper validation.
    const valid: boolean = true;
    if (valid) {
      this.register(this.$data.user);
    }
  }
}
</script>