export class FormFieldModel {
  title: string;
  modelKey: string;
  fieldType: string;
  constructor(title: string, modelKey: string, fieldType: string) {
    this.title     = title     ;
    this.modelKey  = modelKey  ;
    this.fieldType = fieldType ;
  }
}