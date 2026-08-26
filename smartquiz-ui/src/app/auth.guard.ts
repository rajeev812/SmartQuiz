import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = () => {
  const hasToken = Boolean(localStorage.getItem('smartquiz.token'));
  return hasToken || inject(Router).parseUrl('/auth');
};
