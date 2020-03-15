import 'hammerjs';

import {enableProdMode} from '@angular/core';
import {platformBrowserDynamic} from '@angular/platform-browser-dynamic';

import {AppModule} from './app/app.module';
import {environment} from './environments/environment';

import {hmrBootstrap} from './hmr';

declare var module : any;

if (environment.production)
{
  enableProdMode();
}

const bootstrap = () => platformBrowserDynamic().bootstrapModule(AppModule);

if (!environment.production)
{
  if (module['hot'])
  {
    hmrBootstrap(module, bootstrap);
  }
  else
  {
    console.error('HMR is not enabled for webpack-dev-server!');
    console.log('Are you using the --hmr flag for ng serve?');
  }
}
else
{
  bootstrap();
}