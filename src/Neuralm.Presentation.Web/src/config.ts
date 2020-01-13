import { config as configDotenv } from 'dotenv';
import { resolve } from 'path';

switch (process.env.NODE_ENV) {
  case 'development':
    configDotenv({
      path: resolve(__dirname, '/../.env.development')
    });
    break;
  // Add other environments...
  default:
    break;
}

export const HOST = process.env.VUE_APP_MESSAGEQUEUE_HOST;
export const PORT = process.env.VUE_APP_MESSAGEQUEUE_PORT;
