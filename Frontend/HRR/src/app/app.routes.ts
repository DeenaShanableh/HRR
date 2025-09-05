import { Routes } from '@angular/router';
import { Employees } from './component/employees/employees';
import { Departments } from './component/departments/departments';

export const routes: Routes = [
    {path:"", redirectTo:"/employees" , pathMatch : "full"},
    {path:"employees", component:Employees},
    {path:"departments", component:Departments}
];
