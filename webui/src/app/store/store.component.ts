import { Component,  Input, OnInit } from '@angular/core';
import { IDistrict, IStore } from '../domain/models';
import { StoreService } from '../services/store.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';

@Component({
  selector: 'app-store',
  templateUrl: './store.component.html',
  styleUrls: ['./store.component.css']
})
export class StoreComponent implements OnInit {
  @Input() stores: IStore[] = [];
  private _district: IDistrict= {} as IDistrict;
  dialogVisible: boolean;
  storeForm: FormGroup;
  @Input('district')
  set district(value: IDistrict){
    this._district = value;
    this.getStoresForDistrict();
  }
  get district(): IDistrict{
    return this._district;
  }

   constructor(private storeService: StoreService,
      private fb: FormBuilder,
      private msgSvc: MessageService,
      private confirmSvc: ConfirmationService) { 
    
   }
  ngOnInit(): void {
    this.storeForm= this.fb.group({
      storeName:['', Validators.required],
      districtId: 0,
    });
  }

   getStoresForDistrict(){
    if(this.district == null) return;

    this.storeService.getStoresForDistrict(this.district.districtId).subscribe((data: IStore[]) => {
        this.stores = data;
      });
    }
    showDialog(){
      this.dialogVisible = true;
      this.storeForm.patchValue({districtId: this.district.districtId});
    }

    onSubmit(){
      this.storeService.createStore(this.storeForm.value).subscribe((data: IStore) => {
        this.dialogVisible = false;
        this.storeForm.reset();
        this.getStoresForDistrict();
      });
    }
    confirmDelete(event:any, store: IStore){
      console.log(event);
      this.confirmSvc.confirm({
        target: event.target,
        message: `Are you sure you want to delete ${store.storeName} record?`,
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          this.storeService.deleteStore(store).subscribe((data) => {
            this.getStoresForDistrict();
            this.msgSvc.add({severity:'info', summary:'Confirmed', detail:'Record deleted'});
          });
        }
      });
        
    }

}
