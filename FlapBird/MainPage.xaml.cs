namespace FlapBird;

public partial class MainPage : ContentPage

{
    const int gravidade = 3;
    const int tempoEntreFrames = 25;
	const int maxTempoPulando = 3;
	const int forcaPulo = 60;
	const int aberturaMinima = 200;
    bool morto = true;
	double larguraJanela = 0;
    double alturaJanela = 0;
    int velocidade = 20;
	int tempoPulando = 0;
	bool estaPulando = false;
	


    public MainPage()
        {
                InitializeComponent();
        }

    async Task Desenhar()
        {
            while (!morto)
            	{
					if (estaPulando)
					AplicaPulo();
					else
					AplicarGravidade();
                    GerenciaCanos();
					if (VerificaColisao())
					{
						morto = true;
						frameGameOver.IsVisible = true;
						break;
					}
					await Task.Delay(tempoEntreFrames);
            	}
        }

	void AplicaPulo()
	{
		Slime.TranslationY -= forcaPulo;
		tempoPulando++;
		if(tempoPulando >= maxTempoPulando)
		{
			estaPulando = false;
			tempoPulando = 0;
		}
	}

	bool VerificaColisaoteto ()
		{
			var minY = alturaJanela/2;
			if (Slime.TranslationY <= minY)
			return true;
			else
			return false;
		}

	bool VerificaColisaoChao ()
	{
		var maxY = alturaJanela /2	- Chão.HeightRequest;
		if (Slime.TranslationY >=maxY)
		return true;
		else
		return false;
	}

	bool VerificaColisao()
		{
			if(!morto)
			{
				if(VerificaColisaoteto()||
				VerificaColisaoChao())
				{
					return true;
				}
			}
			return false;
		}
	
    void AplicarGravidade()
        {
            Slime.TranslationY += gravidade;
        }

    protected override void OnSizeAllocated(double width, double height)
    	{
        	base.OnSizeAllocated(width, height);
                larguraJanela=width;
                alturaJanela=height;
    	}

    void GerenciaCanos ()
        {
            CanoC.TranslationX-= velocidade;
            CanoB.TranslationX-= velocidade;
                if (CanoB.TranslationX<-larguraJanela)
                	{
                        CanoB.TranslationX=0;
                        CanoB.TranslationX=0;
				var alturaMax = -100;
				var alturaMin = -CanoB.HeightRequest;
				CanoC.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
				CanoB.TranslationY = CanoC.TranslationY + aberturaMinima + CanoB.HeightRequest;
                	}
        }
    void Inicializar()
        {
            morto=false;
            Slime.TranslationY=0;
        }

    void OnGameOverClicked(object s, TappedEventArgs a)
        {
            frameGameOver.IsVisible=false;
            Inicializar();
            Desenhar();

        }

	void OnGridClicked(object s, TappedEventArgs a)
	{
		estaPulando = true;
	}


}


