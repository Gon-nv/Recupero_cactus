import { Component, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { MatPaginator } from "@angular/material/paginator";
import { MatTableDataSource } from "@angular/material/table";
import { HistoricoFichaModel } from "src/app/shared/models/historico-ficha.model";
import { ExcelService } from "src/app/shared/services/excel.service";
import { RecuperoService } from "src/app/shared/services/recupero.service";
import * as FileSaver from 'file-saver';

@Component({
    selector: 'app-historico-fichas',
    templateUrl: './historico-fichas.component.html',
    styleUrls: ['./historico-fichas.component.scss']
  })

export class ConsultarHistoricoFichas implements OnInit{
    public historicoFichaDataSource = new MatTableDataSource<HistoricoFichaModel>();
    public form: FormGroup;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

    constructor(
        private recuperoService: RecuperoService,
        private fb: FormBuilder,
        private excelService: ExcelService

    ){}
    ngOnInit(): void {
        this.form = this.fb.group({
            idFicha: new FormControl()
        });
    }

    buscar(){
        this.recuperoService.obtenerHistoricoFicha(this.form.get('idFicha').value).subscribe(
            (result)=>{
                this.historicoFichaDataSource.data = result;
                this.historicoFichaDataSource.paginator = this.paginator;
            }
        )
    }

    public imprimirExcel(): void {
        this.excelService.exportAsExcelFile(this.historicoFichaDataSource.data, `ficha N°: ${this.form.get('idFicha').value}`);
      }

}