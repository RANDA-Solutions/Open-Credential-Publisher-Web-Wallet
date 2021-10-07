import { RouterModule, Routes } from '@angular/router';
import { SearchRequestComponent } from './pages/search-request/search-request.component';
import { SearchComponent } from './pages/search.component';

export const searchRoutes: Routes = [
  {

    path: '',
	component: SearchComponent,
	children: [{
		path: '',
		component: SearchRequestComponent,
		data: {hideNavBar: true}
	}]
  },
  
];

export const searchRouter = RouterModule.forChild(searchRoutes);
