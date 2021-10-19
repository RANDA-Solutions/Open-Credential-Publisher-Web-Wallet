import { RouterModule, Routes } from '@angular/router';
import { CreateRequestComponent } from './pages/create-request/create-request.component';
import { VerifierComponent } from './pages/verifier.component';
import { ViewComponent } from './pages/view/view.component';

export const verifierRoutes: Routes = [
  {

    path: '',
	component: VerifierComponent,
	children: [{
		path: '',
		component: CreateRequestComponent
	},
	{
		path: ':id', 
		component: ViewComponent
	}]
  },
  
];

export const verifierRouter = RouterModule.forChild(verifierRoutes);
