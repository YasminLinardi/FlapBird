namespace FlapBird;

public partial class MainPage : ContentPage

{
	const int gravidade = 15;
	const int tempoEntreFrames = 50;
	const int maxtempoPulando= 3;
	const int forcaPulo =30;
	const int aberturaMinima=200;
	bool estaPulando = false;
	bool morto = true;
	double larguraJanela = 0;
	double alturaJanela = 0;
	int velocidade = 10;
	int tempoPulando=0;
	int score=0;

	public MainPage()
	{
		InitializeComponent();
	}

	async Task Desenhar()
	{
		while (!morto)
		{
			if (estaPulando)
			AplicarPulo();
			else
			AplicarGravidade();
			GerenciaCanos();
			if (VerificaColisao())
			{
				morto=true;
			//	SoundHelper.Play("morte.wav");
				labelGameOver.Text="Parabéns você passou por \n  "+score.ToString("D3")+" canos";
				frameGameOver.IsVisible=true;
				break;
			}
			await Task.Delay(tempoEntreFrames);
		}
	}

	void AplicarPulo()
	{
		slime.TranslationY-=forcaPulo;
		tempoPulando++;
		if (tempoPulando>= maxtempoPulando)
		{
			estaPulando=false;
			tempoPulando=0;
		}
	}

	bool VerificaColisaoTeto()
	{
		var minY=-alturaJanela/2;
		if (slime.TranslationY <= minY)
		return true;
		else
		return false;

	}
	bool VerificaColisaoChao()
	{
		var maxY=alturaJanela/2 -chao.HeightRequest;
		if (slime.TranslationY >= maxY)
		return true;
		else
		return false;
		
	}

	bool VerificaColisaoCanoCima()
	{
		var posHslime=(larguraJanela-50)-(slime.WidthRequest/2);
		var posVslime=(alturaJanela/2)-(slime.HeightRequest/2)+slime.TranslationY;
		if(posHslime >= Math.Abs(canoC.TranslationX)-canoC.WidthRequest&&
		posHslime <= Math.Abs(canoC.TranslationX)+canoC.WidthRequest&&
		posVslime <= canoC.HeightRequest+canoC.TranslationY)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	bool VerificaColisaoCanoBaixo()
	{
		var posHslime=(larguraJanela-50)-(slime.WidthRequest/2);
		var posVslime=(alturaJanela/2)+(slime.HeightRequest/2)+slime.TranslationY;
		var yMaxCano= canoC.HeightRequest+canoC.TranslationY+aberturaMinima;
		if(posHslime >= Math.Abs(canoB.TranslationX)-canoB.WidthRequest&&
		posHslime <= Math.Abs(canoB.TranslationX)+canoB.WidthRequest&&
        posVslime >= yMaxCano)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	bool VerificaColisao ()
	{
		return VerificaColisaoTeto()||
			VerificaColisaoChao()||
			VerificaColisaoCanoCima()||
			VerificaColisaoCanoBaixo();
	}
	void AplicarGravidade()
	{
		slime.TranslationY += gravidade;
	}

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
		larguraJanela=width;
		alturaJanela=height;
		canoC.HeightRequest=height;
		canoB.HeightRequest=height;
    }

	void GerenciaCanos ()
	{
		canoC.TranslationX-= velocidade;
		canoB.TranslationX-= velocidade;
		if (canoB.TranslationX<-larguraJanela)
		{
			canoB.TranslationX=0;
			canoC.TranslationX=0;
			var alturaMax=-(canoB.HeightRequest*0.1);
			var alturaMin= -canoB.HeightRequest;
			canoC.TranslationY=Random.Shared.Next((int)alturaMin,(int)alturaMax);
			canoB.TranslationY=canoC.TranslationY+aberturaMinima+canoB.HeightRequest;
			labelscore.Text="Canos: "+score.ToString("D3");
			score++;
		//	SoundHelper.Play("pontuacao.wav");
			if(score % 2==0)
			velocidade++;
			
		}
	}
	void Inicializar()
	{
	//	SoundHelper.Play("comeco.wav");
	//	SoundHelper.Play("fundo.wav");
		canoC.TranslationX=-larguraJanela;
		canoB.TranslationX=-larguraJanela;
		slime.TranslationX=0;
	    slime.TranslationY=0;
		score=0;
		morto=false;
		GerenciaCanos();
	}

	void OnFrameClicked(object s, TappedEventArgs a)
	{
		frameGameOver.IsVisible=false;
		Inicializar();
		Desenhar();
	

	}

	void OnGridClicked (object s, TappedEventArgs a)
	{
		estaPulando=true;
	}


}