import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import * as FileSaver from 'file-saver';
import { distinctUntilChanged } from 'rxjs/operators';
import { RegistroArchivoRecuperoModel } from 'src/app/shared/models/registro-archivo-recupero.model';
import { NotificacionService } from 'src/app/shared/notificaciones/notificacion.service';
import { BandejaRecuperoResult } from '../../../shared/models/bandeja-recupero-result.model';
import { BandejaRecupero } from '../../../shared/models/bandeja-recupero.model';
import { Opcion } from '../../../shared/models/opcion';
import { ExcelService } from '../../../shared/services/excel.service';
import { RecuperoService } from '../../../shared/services/recupero.service';

@Component({
  selector: 'app-recupero',
  templateUrl: './recupero.component.html',
  styleUrls: ['./recupero.component.scss']
})
export class RecuperoComponent implements OnInit {
  public form: FormGroup;
  public lsPeriodos: Opcion[] = [];
  public lsProgramas: Opcion[] = [];
  public filtro = new BandejaRecupero();
  public dataSource = new MatTableDataSource<BandejaRecuperoResult>();
  public permiteImprimir: boolean;
  public reglasAplicadas = 'Reglas aplicadas para el recupero: ';
  @ViewChild(MatPaginator, { static: true }) public paginador: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  public fileTxt = document.getElementById('txtFile') as HTMLInputElement;
  public filesToUpload: Array<File>;
  public BandejaRecuperoDataSource = new MatTableDataSource<BandejaRecuperoResult>();
  public bandejaRecupero: BandejaRecuperoResult;
  public registroArchivoList: RegistroArchivoRecuperoModel[] = [];
  public registroArchivo: RegistroArchivoRecuperoModel;

  constructor(
    private formBuilder: FormBuilder,
    private recuperoService: RecuperoService,
    private excelService: ExcelService,
    private notificacionService: NotificacionService
  ) {
  }

  ngOnInit(): void {
    this.recuperoService.obtenerPeriodos().subscribe((res) => {
      this.lsPeriodos = res;
    });
    this.crearFormulario();
  }

  public crearFormulario(): void {
    this.form = this.formBuilder.group({
      nPrograma: [null, Validators.required],
      nPeriodo: [null, Validators.required],
      txConvenio: [null, [Validators.required, Validators.maxLength(5)]],
      dpFecha: [new Date(), [Validators.required]]
    });

    this.form.get('nPeriodo').valueChanges.pipe(distinctUntilChanged()).subscribe((val) => {
      if (val) {
        this.recuperoService.obtenerProgramasPorPeriodo(val).subscribe((res) => {
          this.lsProgramas = res;
        });
      }
    });
  }

  public buscar(): void {
    this.armarConsulta();

    this.recuperoService.obtenerRecuperos(this.filtro).subscribe((res) => {
      this.dataSource.data = res;
      this.customizarPaginador(this.paginador);
      this.dataSource.paginator = this.paginador;
      this.dataSource.paginator.firstPage();
      this.dataSource.sort = this.sort;
      if (res.length) {
        this.recuperoService.puedeGenerarRecupero(this.filtro).subscribe((res) =>
          this.permiteImprimir = res
        );
/*        this.recuperoService.obtenerReglasEtapa(this.filtro.idPrograma).subscribe((reglas) => {
          reglas.forEach((regla) => {
            this.reglasAplicadas += regla.nombre + ', ';
          });
        });*/
      }
    });
  }

  public imprimirExcel(): void {
    const titulo = this.lsPeriodos.find((x) => x.clave == this.form.get('nPeriodo').value).valor;
    const json = JSON.parse(JSON.stringify(this.dataSource.data));
    this.excelService.exportAsExcelFile(json, titulo);
  }

  public imprimir(): void {
    this.recuperoService.obtenerArchivoRecupero(this.filtro).subscribe((archivo) => {
      if (archivo) {
        this.permiteImprimir = false;
        let arrayBytes = this.base64ToBytes(archivo.datos);
        let blob = new Blob([arrayBytes], { type: 'text/plain;charset=utf-8' });
        FileSaver.saveAs(blob, archivo.nombreArchivo);
      }
    });
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

  private armarConsulta(): void {
    this.filtro = new BandejaRecupero();
    this.filtro.idPeriodo = this.form.get('nPeriodo').value;
    const programas = this.form.get('nPrograma').value;
    programas.forEach((id) => {
      if (this.filtro.idPrograma) {
        this.filtro.idPrograma += id.toString() + ',';
      } else {
        this.filtro.idPrograma = id.toString() + ',';
      }
    });
    this.filtro.convenio = this.form.get('txConvenio').value;
    this.filtro.fecha = this.form.get('dpFecha').value;
  }

  public customizarPaginador(paginator: MatPaginator): void {
    paginator._intl.itemsPerPageLabel = 'Recuperos por página';
    paginator._intl.nextPageLabel = 'Siguiente';
    paginator._intl.previousPageLabel = 'Anterior';
    paginator._intl.firstPageLabel = 'Primer página';
    paginator._intl.lastPageLabel = 'Última página';
  }

  getColumnas(): string [] {
    return ['tipoConvenio', 'sucursal', 'moneda', 'sistema', 'nroCuenta', 'importe', 'fecha', 'numeroConvenio', 'numeroComprobante', 'cbu', 'cuota', 'usuario'];
  }

  fileChangeEvent($event) {
    this.readFile($event.target);
  }

  readFile(input) {

    var file: File = input.files[0];
    let reader = new FileReader();

    reader.readAsText(file);
    this.BandejaRecuperoDataSource = new MatTableDataSource<BandejaRecuperoResult>();


    reader.onload = () => {
      this.dividir(reader, file.name)
    }

    reader.onerror = function () {
      console.log(reader.error);
    };
  }

  dividir(reader, fileName) {
    {
      let cadena = (reader.result).toString();
      let saltoLinea = cadena.split('\n');
      for (let i = 0; i < saltoLinea.length - 1; i++) {

        let linea = saltoLinea[i]
        let tipoConvenio = linea.substring(0, 3);
        let sucursal = linea.substring(3, 8);
        let moneda = linea.substring(8, 10);
        let sistema = linea.substring(10, 11);
        let nroCuenta = linea.substring(11, 20);
        let importe = linea.substring(20, 38);
        let fecha = linea.substring(38, 46);
        let numeroConvenio = linea.substring(46, 51);
        let numeroComprobante = linea.substring(51, 57);
        let cbu = linea.substring(57, 79);
        let cuota = linea.substring(79, 81);
        let usuario = linea.substring(81, 103);
        let respBanco = linea.substring(103, 106);
        this.bandejaRecupero = new BandejaRecuperoResult(tipoConvenio, sucursal, moneda, sistema, nroCuenta, importe, fecha, numeroConvenio, numeroComprobante, cbu, cuota, usuario, respBanco)
        this.BandejaRecuperoDataSource.data.push(this.bandejaRecupero);
        this.registroArchivo = new RegistroArchivoRecuperoModel(fileName, usuario, respBanco);
        this.registroArchivoList.push(this.registroArchivo);
      }

    }
    ;

  }

  guardarArchivo() {

    this.recuperoService.guardarArchivo(this.registroArchivoList).subscribe(
      (result) => {
        this.notificacionService.openInfoModal(['Se guardó el archivo'], 'Guardado exitoso');
      },
    );
  }

}
