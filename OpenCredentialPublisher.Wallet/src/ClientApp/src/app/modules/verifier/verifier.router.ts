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
		component: CreateRequestComponent,
		data: {
			hideNavBar: true
		}
	},
	{
		path: ':id', 
		component: ViewComponent,
		data: {
			hideNavBar: true
		}
	}]
  },
  
];

export const verifierRouter = RouterModule.forChild(verifierRoutes);
