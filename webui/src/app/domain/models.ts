interface IDistrict{
    districtId: number;
    districtName: string;
    primarySalesId: number;
    primarySalesFullName: string;
  }
  interface ISalesPerson{
    salesPersonId: number;
    fullName: string;
    salesType: string;
    districtId: number;
  }
  interface IAvailableSalesPerson{
    salesPersonId: number;
    fullName: string;
  }
  interface IAddSalesPerson{
    salesPersonId: number;
    salesType: string;
    districtId: number;
  }
  interface ISecondarySalesPerson{
    districtId: number;
    salesPersonId: number;
    fullName: string;
  }
  interface IStore{
    storeId: number;
    storeName: string;
    districtId: number;
    districtName: string;
}
  export { IDistrict , ISalesPerson ,IAddSalesPerson,IAvailableSalesPerson, ISecondarySalesPerson, IStore};
  