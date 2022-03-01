using Beneficiarios.Aplicacion.Comandos;
using Beneficiarios.Aplicacion.Consultas.Resultados;
using Beneficiarios.Dominio.IRepositorio;
using Beneficiarios.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Datos;
using NHibernate;
using System.Collections.Generic;

namespace Datos.Repositorios.Recupero
{
	public class RecuperoRepositorio : NhRepositorio<Beneficiario>, IRecuperoRepositorio
	{
		public RecuperoRepositorio(ISession sesion) : base(sesion)
		{
		}

		public IList<OpcionResultado> ObtenerPeriodos()
		{
			return Execute("PR_OBTENER_PERIODOS")
				.ToListResult<OpcionResultado>();
		}

		public IList<OpcionResultado> ObtenerProgramasPorPeriodo(int idPeriodo)
		{
			return Execute("PR_OBTENER_ETAPAS_X_PERIODO")
				.AddParam(idPeriodo)
				.ToListResult<OpcionResultado>();
		}

		public IList<OpcionResultado> ObtenerPeriodosPorPrograma(int idPrograma)
		{
			return Execute("PR_OBTENER_PERIODOS_BY_ETAPA")
				.AddParam(idPrograma)
				.ToListResult<OpcionResultado>();
		}

		public IList<OpcionResultado> ObtenerProgramas()
		{
			return Execute("PR_OBTENER_ETAPAS")
				.ToListResult<OpcionResultado>();
		}

		public IList<BandejaRecuperoResultado> ObtenerRecuperos(BandejaRecuperoCommand command, Usuario usuario)
		{
			return Execute("PR_CONSULTA_RECUPERO")
				.AddParam(command.IdPeriodo)
				.AddParam(command.IdPrograma)
				.AddParam(command.Convenio)
				.AddParam(command.Fecha)
				.AddParam(usuario.Cuil)
				.ToListResult<BandejaRecuperoResultado>();
		}

		public IList<RecuperosGeneradosResultado> ObtenerRecuperosGenerados(BandejaRecuperosGeneradosCommand command)
		{
			return Execute("PR_BANDEJA_RECUPERO")
				.AddParam(command.IdPeriodo)
				.AddParam(command.IdPrograma)
				.ToListResult<RecuperosGeneradosResultado>();
		}
		public string ObtenerNombreArchivoRecupero(BandejaRecuperoCommand command, Usuario usuario)
		{
			return Execute("PR_ARCHIVO_RECUPERO")
				.AddParam(command.IdPeriodo)
				.AddParam(command.IdPrograma)
				.AddParam(command.Convenio)
				.AddParam(command.Fecha)
				.AddParam(usuario.Cuil)
				.ToEscalarResult<string>();
		}

		public IList<RegistroArchivoRecupero> ObtenerRegistrosRecuperoImprimir(BandejaRecuperoCommand command, Usuario usuario)
		{
			return Execute("PCK_RECUPERO.PR_OBTENER_ARCHIVO_RECUPERO")
				.AddParam(command.IdPeriodo)
				.AddParam(command.IdPrograma)
				.AddParam(command.Convenio)
				.AddParam(command.Fecha)
				.AddParam(usuario.Cuil)
				.ToListResult<RegistroArchivoRecupero>();
		}

		public RecuperosGeneradosResultado ObtenerCabeceraRecupero(int idRecupero)
		{
			return Execute("PR_OBTENER_CABECERA_RECUPERO")
				.AddParam(idRecupero)
				.ToUniqueResult<RecuperosGeneradosResultado>();
		}

		public IList<BandejaRecuperoResultado> ObtenerDetalleRecupero(int idRecupero)
		{
			return Execute("PR_OBTENER_RECUPERO_BY_ID")
				.AddParam(idRecupero)
				.ToListResult<BandejaRecuperoResultado>();
		}

		public bool PuedeGenerarRecupero(BandejaRecuperoCommand command, Usuario usuario)
		{
			return Execute("PR_PUEDE_GENERAR_RECUPERO")
				.AddParam(command.IdPeriodo)
				.AddParam(command.IdPrograma)
				.AddParam(command.Convenio)
				.AddParam(command.Fecha)
				.AddParam(usuario.Cuil)
				.ToEscalarResult<bool>();
		}

		public IList<ReglaNegocioResultado> ObtenerReglas(int idEtapa)
		{
			return Execute("PR_OBTENER_NOMBRE_REGLAS")
				.AddParam(idEtapa)
				.ToListResult<ReglaNegocioResultado>();
		}

		public IList<RegistroArchivoRecupero> ObtenerRegistrosRecuperoDetalle(int idRecupero)
		{
			return Execute("PR_REIMPRIMIR_RECUPERO_ID")
				.AddParam(idRecupero)
				.ToListResult<RegistroArchivoRecupero>();
		}

		public IList<Cofinanciamiento> ObtenerCofinanciamientosVigentes(int idEtapa)
		{
			return Execute("PR_OBTENER_COFINANCIAMIENTO_X_ETAPA")
				.AddParam(idEtapa)
				.ToListResult<Cofinanciamiento>();
		}

		public Id RegistrarModificacionCofinanciamiento(Cofinanciamiento cofinanciamiento, string cuil)
		{
			return Execute("PCK_RECUPERO.PR_MODIFICAR_COFINANCIAMIENTO")
				.AddParam(cofinanciamiento.Id)
				.AddParam(cofinanciamiento.Nombre)
				.AddParam(cofinanciamiento.CantidadDesde)
				.AddParam(cofinanciamiento.CantidadHasta)
				.AddParam(cofinanciamiento.Monto)
				.AddParam(cuil)
				.ToSpResult().Id;
		}

		public bool RegistrarBajaCofinanciamiento(Cofinanciamiento cofinanciamiento, string cuil)
		{
			return Execute("PCK_RECUPERO.PR_ELIMINAR_COFINANCIAMIENTO")
				.AddParam(cofinanciamiento.Id)
				.AddParam(cuil)
				.ToEscalarResult<bool>();
		}

		public Id RegistrarCofinanciamiento(Cofinanciamiento cofinanciamiento, string cuil)
		{
			return Execute("PCK_RECUPERO.PR_REGISTRAR_COFINANCIAMIENTO")
				.AddParam(cofinanciamiento.Nombre)
				.AddParam(cofinanciamiento.CantidadDesde)
				.AddParam(cofinanciamiento.CantidadHasta)
				.AddParam(cofinanciamiento.Monto)
				.AddParam(cofinanciamiento.IdEtapa)
				.AddParam(cuil)
				.ToSpResult().Id;
		}

		public IList<OpcionResultado> ObtenerEtapasCofinanciamiento()
		{
			return Execute("PR_OBTENER_ETAPAS_COFINANCIAMIENTO")
				.ToListResult<OpcionResultado>();
		}
		public DetalleLiquidacionResultado ObtenerCabeceraDetalleLiquidacion(LiquidacionRecuperoCommand command)
		{
			return Execute("PR_OBTENER_CABECERA_LIQUIDACION")
				.AddParam(command.IdRecupero)
				.AddParam(command.IdEmpresaDebito)
				.ToUniqueResult<DetalleLiquidacionResultado>();
		}

		public IList<DetalleLiquidacion> ObtenerDetalleLiquidacion(LiquidacionRecuperoCommand command)
		{
			return Execute("PR_OBTENER_DETALLE_LIQUIDACION")
				.AddParam(command.IdRecupero)
				.AddParam(command.IdEmpresaDebito)
				.ToListResult<DetalleLiquidacion>();
		}

		public IList<HistoricoFichasResultado> ObtenerHistoricoFicha(int idFicha)
        {
			return Execute("PR_OBTENER_HISTORICO_FICHAS")
				.AddParam(idFicha)
				.ToListResult<HistoricoFichasResultado>();
        }

		public IList<OpcionResultado> ObtenerEtapasReglasNegocio()
		{
			return Execute("PR_OBTENER_ETAPAS_REGLAS_NEGOCIO")
				.ToListResult<OpcionResultado>();
		}

		public IList<OpcionResultado> ObtenerTipoPPPReglasNegocio()
		{
			return Execute("PR_OBTENER_TIPOS_PPP")
				.ToListResult<OpcionResultado>();
		}

		public IList<BandejaReglasNegocioResultado> ObtenerReglasNegocio(BandejaReglaNegocioCommand command)
		{
			return Execute("OBTENER_REGLAS_NEGOCIO_X_FILTRO")
				.AddParam(command.IdEtapa)
				.AddParam(command.IdTipoPPP)
				.ToListResult<BandejaReglasNegocioResultado>();
		}

		public IList<OpcionResultado> ObtenerEtapas()
		{
			return Execute("PR_OBTENER_ETAPAS")
				.ToListResult<OpcionResultado>();
		}

		public IList<OperadorResultado> ObtenerOperadores()
		{
			return Execute("PR_OBTENER_OPERADORES")
				.ToListResult<OperadorResultado>();
		}

		public IList<OpcionResultado> ObtenerCondiciones()
		{
			return Execute("PR_OBTENER_CONDICIONES")
				.ToListResult<OpcionResultado>();
		}

        public Id RegistrarReglaNegocio(ReglaNegocioCommand command, string Cuil)
        {
			return Execute("PCK_REGLA_NEGOCIO.PR_REGISTRAR_REGLA_NEGOCIO")
				.AddParam(command.Nombre)
				.AddParam(command.IdCondicion)
				.AddParam(command.IdOperadorComparacion)
				.AddParam(command.ValorComparacion)
				.AddParam(command.IdSexo)
				.AddParam(command.IdOperadorAsignacion)
				.AddParam(command.ValorAsignacion)
				.AddParam(command.IdEtapa)
				.AddParam(Cuil)
				.ToSpResult().Id;
		}
		public int GuardarArchivoRecupero(Archivo command)
        {
			return Execute("PR_GUARDAR_ARCHIVO_RECUPERO")
				.AddParam(command.NombreArchivo)
				.AddParam(command.Usuario)
				.AddParam(command.respBanco)
				.ToEscalarResult<int>();
        }

		public Id EliminarRecupero(int idRecupero)
        {
			return Execute("PCK_RECUPERO.PR_ELIMINAR_RECUPERO")
				.AddParam(idRecupero)
				.ToSpResult().Id;
        }


		public Id ModificarReglaNegocio(ReglaNegocioCommand command, string Cuil)
		{
			return Execute("PCK_REGLA_NEGOCIO.PR_MODIFICAR_REGLA_NEGOCIO")
				.AddParam(command.Id)
				.AddParam(command.Nombre)
				.AddParam(command.IdCondicion)
				.AddParam(command.IdOperadorComparacion)
				.AddParam(command.ValorComparacion)
				.AddParam(command.IdSexo)
				.AddParam(command.IdOperadorAsignacion)
				.AddParam(command.ValorAsignacion)
				.AddParam(command.IdEtapa)
				.AddParam(Cuil)
				.ToSpResult().Id;
		}

		public Id EliminarReglaNegocio(ReglaNegocioCommand command, string Cuil)
		{
			return Execute("PCK_REGLA_NEGOCIO.PR_ELIMINAR_REGLA_NEGOCIO")
				.AddParam(command.Id)
				.AddParam(Cuil)
				.ToSpResult().Id;
		}

		public ReglaNegocioCommand ObtenerReglaNegocio(int id)
        {
			return Execute("PR_OBTENER_REGLA_NEGOCIO_X_ID")
				.AddParam(id)
				.ToUniqueResult<ReglaNegocioCommand>();
		}
	}
}
