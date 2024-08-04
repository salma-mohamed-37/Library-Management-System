import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

export const roleGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const role = route.data['role']

  if(role == authService.getCurrentUserRole())
  {
    return true;
  }
  else
  {
    //router.navigate(['/unauthorized']);
    return false;
  }

};
