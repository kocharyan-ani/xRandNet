import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { environment } from './environments/environment';
import {XRandNetModule} from "./app/x-rand-net/x-rand-net.module";

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(XRandNetModule)
  .catch(err => console.error(err));

