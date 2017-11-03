export class Globals {
    public static API_URL: string = "http://localhost:49871";

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
}

export class ImportPartData {
    id: number = -1;
    name:string = "";
    setdefault: boolean = false;
}