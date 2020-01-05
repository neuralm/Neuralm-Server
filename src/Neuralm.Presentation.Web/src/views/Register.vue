<template>
  <div id="register">
    <div id="description">
      <h1>Register</h1>
      <p>By registering you agree to the ridiculously long terms that you didn't bother to read.</p>
    </div>
    <div id="form">
      <ValidationObserver v-slot="{ handleSubmit }">
        <form @submit.prevent="handleSubmit(onSubmit)">
          <div class="form-group">
            <v-text-field-with-validation rules="required|min:3" v-model="username" label="Username" autocomplete="off"/>
          </div>
          <div class="form-group">
            <v-text-field-with-validation rules="required|password" v-model="password" label="Password" type="password"/>
          </div>
          <div class="form-group">
            <button class="btn btn-primary" :disabled="!username && !password">Register</button>
            <img
              v-show="status.registering"
              src="data:image/gif;base64,R0lGODlhEAAQAPIAAP///wAAAMLCwkJCQgAAAGJiYoKCgpKSkiH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA=="
            />
            <router-link to="/login" class="btn btn-link">Cancel</router-link>
          </div>
        </form>
      </ValidationObserver>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Inject } from 'vue-property-decorator';
import { mapState, mapActions } from 'vuex';
import IUserService from '../interfaces/IUserService';
import User from '../models/User';
import RegisterRequest from '../messages/requests/RegisterRequest';
import RegisterResponse from '../messages/responses/RegisterResponse';
import ComponentLoader from '@/helpers/ComponentLoader';
import { ValidationObserver } from 'vee-validate';

@Component({
  components: {
    VTextFieldWithValidation: () => ComponentLoader('inputs/VTextFieldWithValidation'),
    ValidationObserver
  },
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
        if (response.success) {
          this.$store.commit('user/registerSuccess');
          this.$snotify.success(response.message);
          this.$router.push('/login');
        } else {
          this.$store.commit('user/registerFailure');
          this.$snotify.error(response.message);
        }
      })
      .catch((error: Promise<RegisterResponse> | RegisterResponse) => {
        this.$store.commit('user/registerFailure');
        if (error instanceof RegisterResponse) {
          this.$snotify.error(error.message);
        } else {
          this.$snotify.error('Something happened try again later.');
        }
      }
    );
  }

  public onSubmit(e: Event) {
    this.$data.submitted = true;
    const valid: boolean = true;
    if (valid) {
      this.register(this.$data.username, this.$data.password);
    }
  }
}
</script>
<style scoped>
* {
  box-sizing: border-box;
}

html,
body {
  height: 100%;
  margin: 0;
  padding: 0;
  width: 100%;
}

div#app {
  width: 100%;
  height: 100%;
}

div#app div#register {
  align-items: center;
  display: flex;
  justify-content: center;
  width: 100%;
  height: 100%;
}

div#app div#register div#description {
  background-color: #ffffff;
  width: 280px;
  padding: 35px;
}

div#app div#register div#description h1,
div#app div#register div#description p {
  margin: 0;
}

div#app div#register div#description p {
  font-size: 0.8em;
  color: #95a5a6;
  margin-top: 10px;
}

div#app div#register div#form {
  background-color: #34495e;
  border-radius: 5px;
  box-shadow: 0px 0px 30px 0px #666;
  color: #ecf0f1;
  width: 260px;
  padding: 35px;
}

div#app div#register div#form label,
div#app div#register div#form input {
  outline: none;
  width: 100%;
}

div#app div#register div#form label {
  color: #95a5a6;
  font-size: 0.8em;
}

div#app div#register div#form input {
  background-color: transparent;
  border: none;
  color: #ecf0f1;
  font-size: 1em;
  margin-bottom: 20px;
}

div#app div#register div#form ::placeholder {
  color: #ecf0f1;
  opacity: 1;
}

div#app div#register div#form button {
  background-color: #e2e2e5;
  color: black;
  font-weight: bold;
  cursor: pointer;
  border: none;
  padding: 10px;
  transition: background-color 0.2s ease-in-out;
  width: 100%;
}

div#app div#register div#form button:hover {
  background-color: #eeeeee;
}

@media screen and (max-width: 600px) {
  div#app div#register {
    align-items: unset;
    background-color: unset;
    display: unset;
    justify-content: unset;
  }

  div#app div#register div#description {
    margin: 0 auto;
    max-width: 350px;
    width: 100%;
  }

  div#app div#register div#form {
    border-radius: unset;
    box-shadow: unset;
    width: 100%;
  }

  div#app div#register div#form form {
    margin: 0 auto;
    max-width: 280px;
    width: 100%;
  }
}
</style>