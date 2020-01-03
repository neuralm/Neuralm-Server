import { required, max, min } from 'vee-validate/dist/rules';
import { extend } from 'vee-validate';

extend('required', {
  ...required,
  message: 'This field is required'
});

extend('max', {
  ...max,
  message: 'This field must be {length} characters or less'
});

extend('min', {
  ...min,
  message: 'This field must be {length} characters or more'
});
