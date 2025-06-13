using System;
using System.IO;
using System.Runtime.Intrinsics.Arm;

namespace MSader.DTO
{
    public class MidiaDTO : TipoMidiaDTO
    {
        #region Propriedades 

        public int IDMidia { get; set; }

        public int IDPostMidia { get; set; }

        public string? NMTitulo { get; set; }

        public string? CDEmbedded { get; set; }

        public string? DSLegenda { get; set; }

        public string? DSUrlMidia { get; set; }

        public string? NMFileName { get; set; }

        public byte[]? IGArquivo { get; set; }

        public string? CDExtensao { get; set; }

        public string? FolderName { get; set; }

        public string? FullPath { get; set; }

        public int NROrdem { get; set; }

        public bool STMidiaMain { get; set; }

        #endregion

        #region Construtores 

        public MidiaDTO()
        { }

        public MidiaDTO(string urlBase, int idPost, int nrOrdemPost)
        {
            DSUrlMidia = $"{urlBase}{ConstantsDTO.PATH_FOTOS}{idPost}/{idPost}-{nrOrdemPost}.png";
                
            NROrdem = nrOrdemPost ;
        }

        public MidiaDTO(string fileName, byte[] fileBytes, string filePartsLast, string nmTitulo, string dsLegenda, string cdEmbedded)
        {
            NMFileName = fileName;
            IGArquivo = fileBytes;
            CDExtensao = filePartsLast;
            STMidiaMain = false;
            NROrdem = 1;
            NMTitulo = nmTitulo;
            DSLegenda = dsLegenda;
            CDEmbedded = cdEmbedded;

            switch (filePartsLast)
            {
                case "png":
                    IDTipoMidia = 1;
                    break;
                case "mp4":
                    IDTipoMidia = 2;
                    break;
                case "mp3":
                    IDTipoMidia = 3;
                    break;
                default:
                    IDTipoMidia = 1;
                    break;
            }
        }

        #endregion

        #region Métodos 

        public void SetFolderName(int idPost)
        {
            FolderName = $"{ConstantsDTO.PATH_FOTOS}{idPost}/"; 
        }

        #endregion
    }

    public class TipoMidiaDTO
    {
        #region Propriedades 
        public int IDTipoMidia { get; set; }
        
        public string? NMTipoMidia { get; set; }

        public string? CDExtencaoArquivo { get; set; }

        #endregion
        
        #region Construtores 
        
        public TipoMidiaDTO()
        { }
        
        #endregion
        
        #region Métodos 
        #endregion
    }
}
