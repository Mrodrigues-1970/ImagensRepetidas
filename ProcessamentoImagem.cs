using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ImagensRepetidas
{
    public static class ProcessamentoImagem
    {


        public static ImagemAnalizada RetornaHashMedio(string filePath)
        {
            try { 
                ImagemAnalizada oImagem = new ImagemAnalizada();
                using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(filePath))
                {
                    oImagem.Altura = image.Height;
                    oImagem.Largura = image.Width;
                    oImagem.PathCompleto = filePath;
                    // Reduz para 8x8 em tons de cinza
                    image.Mutate(x => x.Resize(8, 8).Grayscale());

                    double total = 0;
                    double[] pixels = new double[64];
                    int i = 0;

                    for (int y = 0; y < 8; y++)
                    {
                        for (int x = 0; x < 8; x++)
                        {
                            var pixel = image[x, y];
                            double value = pixel.R; // já está em grayscale
                            pixels[i++] = value;
                            total += value;
                        }
                    }

                    double avg = total / 64.0;
                    char[] bits = new char[64];

                    for (int j = 0; j < 64; j++)
                    {
                        bits[j] = pixels[j] >= avg ? '1' : '0';
                    }
                    oImagem.Hash = new string(bits);
                    return oImagem;
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar {filePath}: {ex.Message}");
                return null;
            }
        }







    }
}
