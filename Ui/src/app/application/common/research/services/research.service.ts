import {Injectable} from '@angular/core';
import {ResearchEnum} from "../enums/research-enum";
import {Observable, of} from "rxjs";
import {BaseResearch} from "../models/base-research";
import {HttpService} from "../../../../common/http/services/http.service";

@Injectable({
    providedIn: 'root'
})
export class ResearchService {

    private researches: BaseResearch[] = [];

    constructor(private httpService: HttpService) {
    }

    public startResearch(research: BaseResearch): Observable<string> {
        return this.httpService.post('api/research/start', {}, research)
    }

    public download(path: string): Observable<Blob> {
        return this.httpService.blob(`api/research/download`, {path})
    }

    public downloadFolder(path: string): Observable<Blob> {
        return this.httpService.blob(`api/research/downloadFolder`, {path})
    }

    public getResearches(): Observable<BaseResearch[]> {
        return of(this.researches);
    }

    public addResearch(research: BaseResearch): Observable<boolean> {
        this.researches.push(research);
        return of(true)
    }

    public getResearchNames(): ResearchEnum[] {
        return Object.keys(ResearchEnum).map(key => ResearchEnum[key])
    }
}
