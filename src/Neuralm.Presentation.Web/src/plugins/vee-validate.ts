import { required, max, min, max_value, min_value } from 'vee-validate/dist/rules';
import { extend } from 'vee-validate';

extend('max_value', {
  ...max_value,
  message: 'The value exceeds the maximum.'
});
extend('min_value', {
  ...min_value,
  message: 'The value is below the minimum.'
});
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
extend('password', {
  message: 'Password should contain upper-, lowercase letter, number, and be of a minimum length of 9.',
  validate: (value) => /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{9,64}$/.test(value)
});
