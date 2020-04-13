import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {BaseResearch} from "../../common/research/models/base-research";
import {MatSnackBar} from "@angular/material/snack-bar";
import {ResearchService} from "../../common/research/services/research.service";
import {switchMap} from "rxjs/operators";
import {saveAs} from 'file-saver';
import {StorageTypeEnum} from "../../common/research/enums/storage-type-enum";

@Component({
    selector: 'app-research-landing',
    templateUrl: './research-landing.component.html',
    styleUrls: ['./research-landing.component.css']
})
export class ResearchLandingComponent implements OnInit {

    public selectedResearch: BaseResearch;

    constructor(
        private router: Router,
        private matSnackBar: MatSnackBar,
        private researchService: ResearchService
    ) {
    }

    ngOnInit() {
    }

    public createResearch() {
        this.router.navigate(['application', 'create-research']);
    }

    public selectResearch(research: BaseResearch) {
        this.selectedResearch = research;
    }

    public startResearch() {
        if (this.selectedResearch) {
            let storage = this.selectedResearch.storage;
            let fileName = "";
            this.researchService.startResearch(this.selectedResearch).pipe(
                switchMap(res => {
                    if (storage == StorageTypeEnum.TEXT) {
                        fileName = res + ".zip";
                        return this.researchService.downloadFolder(res)
                    } else {
                        fileName = res;
                        return this.researchService.download(res)
                    }
                })
            ).subscribe((res: Blob) => {
                saveAs(res, fileName)
            })
        } else
            this.matSnackBar.open('Please select a research.');
    }
}
