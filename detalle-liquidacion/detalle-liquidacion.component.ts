import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { DetalleLiquidacionResultModel } from '../../../../shared/models/detalle-liquidacion.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import * as FileSaver from 'file-saver';
import { RecuperoService } from '../../../../shared/services/recupero.service';
import { ExcelService } from '../../../../shared/services/excel.service';
import { LiquidacionRecuperoCommand } from 'src/app/shared/models/liquidacion-recupero-command.model';
import { BandejaDetalleLiquidacionResult } from 'src/app/shared/models/bandeja-detalle-liquidacion-result';


@Component({
  selector: 'app-detalle-liquidacion',
  templateUrl: './detalle-liquidacion.component.html',
  styleUrls: ['./detalle-liquidacion.component.scss']
})
export class DetalleLiquidacionComponent implements OnInit {
  public filtro = new LiquidacionRecuperoCommand();
  public liquidacion = new BandejaDetalleLiquidacionResult();
  public dataSource = new MatTableDataSource<DetalleLiquidacionResultModel>();
  @ViewChild(MatPaginator, { static: true }) public paginador: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  
  
  constructor(private recuperoService: RecuperoService,
              private router: Router,
              private route: ActivatedRoute,
              private excelService: ExcelService,
              ) {
  }


  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.filtro.idRecupero = +params['idRecupero'];
      this.filtro.idEmpresaDebito = +params['idEmpresaDebito'];    
        this.recuperoService.obtenerDetalleLiquidacion(this.filtro).subscribe((recupero) => {
          this.liquidacion = recupero;
          this.dataSource.paginator = this.paginador;
          this.dataSource.data = this.liquidacion.detalle;
          this.dataSource.sort = this.sort;
        });
      }
    );
  }
  public imprimirExcel(): void {
    const titulo = 'Liquidacion ';
    //  + ' - Periodo ' + this.liquidacion.periodo;
    const json = JSON.parse(JSON.stringify(this.dataSource.data));
    this.excelService.exportAsExcelFile(json, titulo);
  }

  public imprimir(): void {
    this.recuperoService.obtenerArchivoRecuperoDetalle(this.liquidacion.idEmpresaDebito).subscribe((archivo) => {
      if (archivo) {
        let arrayBytes = this.base64ToBytes(archivo.datos);
        let blob = new Blob([arrayBytes], { type: 'text/plain;charset=utf-8' });
        FileSaver.saveAs(blob, this.liquidacion.cbu);
      }
    })
  }

  private base64ToBytes(base64) {
    let raw = window.atob(base64);
    let n = raw.length;
    let bytes = new Uint8Array(new ArrayBuffer(n));
    for (let i = 0; i < n; i++) {
      bytes[i] = raw.charCodeAt(i);
    }
    return bytes;
  }
  
  getColumnas(): string [] {
    return ['apellido', 'nombre', 'nombreSexo', 'programa', 'cuil', 'importe', 'fechaPago'];
  }


}
