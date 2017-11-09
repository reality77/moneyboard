import { IDataIdName } from '../common/interfaces';

export class Globals {
    public static API_URL: string = "http://localhost:49871";

    private static _formatMonthYear = new Intl.DateTimeFormat(["fr-FR"], {
        month: "long",
        year: "numeric"
    });   

    private static _formatDate = new Intl.DateTimeFormat(["fr-FR"], {
        day: "numeric",
        month: "numeric",
        year: "numeric"
    });   

    public static sortDataList(datalist: Array<IDataIdName>): Array<IDataIdName> {

        var result = datalist.slice() as Array<IDataIdName>;

        result.sort((a: any, b: any) => {
            if (a.name == b.name)
                return 0;
            else if (a.name < b.name)
                return -1;
            else
                return 1;
        });

        return result;
    }

    public static formatDateMonthYear(date: Date): string {

        return this._formatMonthYear.format(date);
    }

    public static formatDate(date: Date): string {

        return this._formatDate.format(date);
    }
}

export class ImportPartData {
    id: number = -1;
    name:string = "";
    setdefault: boolean = false;
}