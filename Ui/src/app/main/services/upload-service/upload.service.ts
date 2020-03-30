import {Injectable} from '@angular/core';
import {HttpClient, HttpEventType, HttpRequest, HttpResponse} from '@angular/common/http';
import {Observable, Subject} from 'rxjs';

@Injectable()
export class UploadService {
    constructor(private http: HttpClient) {
    }

    public upload(file: File, url: string): Observable<number> {
        const formData: FormData = new FormData();
        formData.append('file', file, file.name);
        let req = new HttpRequest('POST', url, formData, {
            reportProgress: true
        });
        const progress = new Subject<number>();
        this.http.request(req).subscribe(event => {
            if (event.type === HttpEventType.UploadProgress) {
                const percentDone = Math.round((100 * event.loaded) / event.total);
                progress.next(percentDone);
            } else if (event instanceof HttpResponse) {
                progress.complete();
            }
        });
        return progress.asObservable();
    }
}
