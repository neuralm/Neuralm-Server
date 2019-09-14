<template>
  <div id="login">
    <div id="description">
      <h1>Login</h1>
      <p>By logging in you agree to the ridiculously long terms that you didn't bother to read.</p>
    </div>
    <div id="form">
      <form @submit.prevent="handleSubmit">
        <div class="form-group">
          <label for="username">Username</label>
          <input type="text" v-model="username" name="username" class="form-control" :class="{ 'is-invalid': submitted && !username }" autocomplete="off" />
          <div v-show="submitted && !username" class="invalid-feedback">Username is required</div>
        </div>
        <div class="form-group">
          <label htmlFor="password">Password</label>
          <input type="password" v-model="password" name="password" class="form-control" :class="{ 'is-invalid': submitted && !password }" />
          <div v-show="submitted && !password" class="invalid-feedback">Password is required</div>
        </div>
        <div class="form-group">
          <button class="btn btn-primary" :disabled="!username && !password">Login</button>
          <router-link to="/register" class="btn btn-link">Register</router-link>
        </div>
      </form>
    </div>
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
    ...mapState('user', ['status'])
  }
})
export default class LoginView extends Vue {
  @Prop() private userService!: IUserService;

  public login(username: string, password: string): void {
    this.$store.commit('user/loginRequest', { username });
    this.userService.login(new AuthenticateRequest(username, password)).then(
      (response: AuthenticateResponse) => {
        if (response.success) {
          const user: User = { username, userId: response.userId, accessToken: response.accessToken };
          this.$store.commit('user/loginSuccess', user);
          this.$snotify.success('Successfully logged in!');
          this.$router.push('/');
        } else {
          this.$store.commit('user/loginFailure');
          this.$snotify.error('Incorrect credentials!');
        }
      },
      (error: Promise<AuthenticateResponse>) => {
        this.$store.commit('user/loginFailure');
        error.then((value: AuthenticateResponse) => {
          this.$snotify.error(value.message);
        });
      }
    );
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

div#app div#login {
  align-items: center;
  display: flex;
  justify-content: center;
  width: 100%;
  height: 100%;
}

div#app div#login div#description {
  background-color: #ffffff;
  width: 280px;
  padding: 35px;
}

div#app div#login div#description h1,
div#app div#login div#description p {
  margin: 0;
}

div#app div#login div#description p {
  font-size: 0.8em;
  color: #95a5a6;
  margin-top: 10px;
}

div#app div#login div#form {
  background-color: #34495e;
  border-radius: 5px;
  box-shadow: 0px 0px 30px 0px #666;
  color: #ecf0f1;
  width: 260px;
  padding: 35px;
}

div#app div#login div#form label,
div#app div#login div#form input {
  outline: none;
  width: 100%;
}

div#app div#login div#form label {
  color: #95a5a6;
  font-size: 0.8em;
}

div#app div#login div#form input {
  background-color: transparent;
  border: none;
  color: #ecf0f1;
  font-size: 1em;
  margin-bottom: 20px;
}

div#app div#login div#form ::placeholder {
  color: #ecf0f1;
  opacity: 1;
}

div#app div#login div#form button {
  background-color: #e2e2e5;
  color: black;
  font-weight: bold;
  cursor: pointer;
  border: none;
  padding: 10px;
  transition: background-color 0.2s ease-in-out;
  width: 100%;
}

div#app div#login div#form button:hover {
  background-color: #eeeeee;
}

@media screen and (max-width: 600px) {
  div#app div#login {
    align-items: unset;
    background-color: unset;
    display: unset;
    justify-content: unset;
  }

  div#app div#login div#description {
    margin: 0 auto;
    max-width: 350px;
    width: 100%;
  }

  div#app div#login div#form {
    border-radius: unset;
    box-shadow: unset;
    width: 100%;
  }

  div#app div#login div#form form {
    margin: 0 auto;
    max-width: 280px;
    width: 100%;
  }
}
</style>