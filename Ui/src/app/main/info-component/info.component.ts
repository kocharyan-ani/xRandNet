import {Component, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Title} from "@angular/platform-browser";
import {Link} from "../../common/models/link";
import {Person} from "../../common/models/person";


@Component({
    selector: "info-page",
    templateUrl: 'info.component.html',
    styleUrls: ["info.component.css"]
})
export class InfoComponent implements OnInit {

    people: Array<Person> = Array<Person>();
    literatureLinks: Array<Link> = Array<Link>();


    constructor(private titleService: Title, private httpClient: HttpClient) {

    }

    ngOnInit(): void {
        this.titleService.setTitle('Info');
        // this.httpClient.get(apiUrl + '/api/public/people/all')
        //     .subscribe((people: Array<Person>) => {
        //         if (people != null && people.length != 0) {
        //             this.people = people;
        //         }
        //     });
        // const params = new HttpParams().set('type', LinkType.LITERATURE.toString());
        // this.httpClient.get(apiUrl + '/api/data/links', {params})
        //     .subscribe((links: Array<Link>) => {
        //         if (links != null && links.length != 0) {
        //             this.literatureLinks = links;
        //         }
        //     });
    }

}
