import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { RecuperoService } from '../../../../shared/services/recupero.service';
import { RecuperosGeneradosResultModel } from '../../../../shared/models/recuperos-generados-result.model';
import { BandejaRecuperoResult } from '../../../../shared/models/bandeja-recupero-result.model';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import * as FileSaver from 'file-saver';
import { ExcelService } from '../../../../shared/services/excel.service';


@Component({
  selector: 'app-detalle-recupero',
  templateUrl: './detalle-recupero.component.html',
  styleUrls: ['./detalle-recupero.component.scss']
})
export class DetalleRecuperoComponent implements OnInit {
  public idRecupero: number;
  public recupero = new RecuperosGeneradosResultModel();
  public dataSource = new MatTableDataSource<BandejaRecuperoResult>();
  @ViewChild(MatPaginator, { static: true }) public paginador: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private formBuilder: FormBuilder,
              private recuperoService: RecuperoService,
              private router: Router,
              private route: ActivatedRoute,
              private excelService: ExcelService) {
  }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.idRecupero = +params['id'];
      
      if (this.idRecupero) {
        this.recuperoService.obtenerDetalleRecupero(this.idRecupero).subscribe((recupero) => {
          this.recupero = recupero;
          this.dataSource.data = this.recupero.detalle;
          this.customizarPaginador(this.paginador);
          this.dataSource.paginator = this.paginador;
          this.dataSource.paginator.firstPage();
          this.dataSource.sort = this.sort;
        });
      }
    });
  }
  public customizarPaginador(paginator: MatPaginator): void {
    paginator._intl.itemsPerPageLabel = 'Beneficiarios por página';
    paginator._intl.nextPageLabel = 'Siguiente';
    paginator._intl.previousPageLabel = 'Anterior';
    paginator._intl.firstPageLabel = 'Primer página';
    paginator._intl.lastPageLabel = 'Última página';
  }

  public imprimirExcel(): void {
    const titulo = 'Recupero ' + this.recupero.idRecupero + ' - Periodo ' + this.recupero.periodo;
    const json = JSON.parse(JSON.stringify(this.dataSource.data));
    this.excelService.exportAsExcelFile(json, titulo);
  }

  public imprimir(): void {
    this.recuperoService.obtenerArchivoRecuperoDetalle(this.recupero.idRecupero).subscribe((archivo) => {
      if (archivo) {
        let arrayBytes = this.base64ToBytes(archivo.datos);
        let blob = new Blob([arrayBytes], { type: 'text/plain;charset=utf-8' });
        FileSaver.saveAs(blob, this.recupero.nombreArchivo);
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

  public volver(): void {
    this.router.navigate(['gestion', 'bandeja-recuperos-generados']);
  }

  getColumnas(): string [] {
    return ['tipoConvenio', 'sucursal', 'moneda', 'sistema', 'nroCuenta', 'importe', 'fecha', 'numeroComprobante', 'cbu', 'cuota', 'usuario', 'detalleLiquidacion',];
  }

  public verDetalleLiquidacion(idEmpresaDebito: number): void {
    this.router.navigate(['gestion', 'detalle-liquidacion', this.idRecupero, idEmpresaDebito]);
  }
}
