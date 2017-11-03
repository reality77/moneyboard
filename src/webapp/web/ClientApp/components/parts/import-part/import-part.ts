/// <reference path='../../common/interfaces.ts'/>

import Vue from 'vue';
import { Component, Inject, Model, Prop, Provide, Watch } from 'vue-property-decorator'
import { Globals, ImportPartData } from '../../common/globals';

@Component({model: {
    prop: 'value',
    event: 'input'
  }})
export default class ImportPart extends Vue {

    @Prop()
    placeholder: String;
    @Prop()
    forcesetdefault: Boolean;
    @Prop()
    selectedname: String;
    @Prop()
    datalist: Array<any>;
    @Prop()
    value: ImportPartData;
    @Prop()
    noid: boolean;

    private initialNewName:string = "";

    id:number = -1;
    name:string = "";
    setdefault:boolean = false;

    @Watch('value')
    onValueChanged(oldValue:any, newValue:any) {
        console.log("onValueChanged() : " + newValue);
        this.id = newValue.id;
        this.name = newValue.name;
        this.setdefault = newValue.setdefault;

        if(this.forcesetdefault)
        {
            if(!this.setdefault) {
                this.setdefault = true;
                this.updateValue();
            }
        }
    }

    mounted() {
        this.id = this.value.id;
        this.initialNewName = this.value.name;
        if((this.selectedname != null && this.selectedname != "") && (this.initialNewName == null || this.initialNewName == "" )) {
            this.initialNewName = this.selectedname.toString();
            this.updateValue();
        }

        this.name = this.initialNewName;
        this.setdefault = this.value.setdefault;
    
        if(this.forcesetdefault)
        {
            if(!this.setdefault) {
                this.setdefault = true;
                this.updateValue();
            }
        }
    }

    createNew() {
        this.id = -1;
        this.name = this.initialNewName;

        this.updateValue();
    }

    selectExisting(data:any) {
        this.id = data.id;
        this.name = data.name;

        this.updateValue();
    }

    onInputChanged() {
        this.updateValue();
    }

    onDefaultChecked() {
        console.log("checked");
        this.updateValue();
    }

    updateValue() {
        var val = {id: this.id, name: this.name, setdefault: this.setdefault };
        console.log(val);
        this.$emit('input', val);
    }
}