import { Component, EventEmitter, Input ,Output } from '@angular/core';

@Component({
  selector: 'app-confirmation-dialog',
  imports: [],
  templateUrl: './confirmation-dialog.html',
  styleUrl: './confirmation-dialog.css'
})
export class ConfirmationDialog {
@Input() title : string = ""; //Get value Form Parent  component
@Input() body : string = "";  //Get value Form Parent  component

@Output()  confirm = new EventEmitter<boolean>();

confirmDelete(isConfirmed : boolean ){
  this.confirm.emit(isConfirmed); //Activate confiem event, and transfer value to parent component
}
}
