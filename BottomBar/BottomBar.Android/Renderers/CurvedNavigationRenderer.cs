using Android.Content;
using Android.Graphics;
using BottomBar.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using Point = Android.Graphics.Point;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(CurvedNavigationRenderer))]
namespace BottomBar.Droid.Renderers
{
    public class CurvedNavigationRenderer : TabbedPageRenderer
    {
        private Path mPath;
        private Paint mPaint;

        /** the CURVE_CIRCLE_RADIUS represent the radius of the fab button */
        private int CURVE_CIRCLE_RADIUS = 256 / 2;
        // the coordinates of the first curve
        private Point mFirstCurveStartPoint = new Point();
        private Point mFirstCurveEndPoint = new Point();
        private Point mFirstCurveControlPoint1 = new Point();
        private Point mFirstCurveControlPoint2 = new Point();

        //the coordinates of the second curve
        //@SuppressWarnings("FieldCanBeLocal")
        private Point mSecondCurveStartPoint = new Point();
        private Point mSecondCurveEndPoint = new Point();
        private Point mSecondCurveControlPoint1 = new Point();
        private Point mSecondCurveControlPoint2 = new Point();
        private int mNavigationBarWidth;
        private int mNavigationBarHeight;

        //Android
        private int tabHeight = 170;
        //Razlika izmedju vrha ekrana i tabova
        private int differenceOfScreen = 0;


        public CurvedNavigationRenderer(Context context) : base(context)
        {
            Initialeze();
        }

        ///TODO: Trenutno mislim da se ne vidi zato sto se visine ne poklapaju kao sto je samo kod android.
        ///Mora da se lepo testira, kad se napise velicina od 216 nista se ne vidi, 
        ///a kad ostane njena prava visina onda ga oboji u zeleno

        public void Initialeze()
        {
            mPath = new Path();
            mPaint = new Paint();
            mPaint.SetStyle(Paint.Style.FillAndStroke);
            mPaint.Color = Android.Graphics.Color.LightBlue;
            SetBackgroundColor(Android.Graphics.Color.Transparent);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            // get width and height of navigation bar
            // Navigation bar bounds (width & height)
            mNavigationBarWidth = Width;
            mNavigationBarHeight = Height;
            differenceOfScreen = Height - tabHeight;

            // the coordinates (x,y) of the start point before curve
            //mFirstCurveStartPoint.Set((mNavigationBarWidth / 2) - (CURVE_CIRCLE_RADIUS * 2) - (CURVE_CIRCLE_RADIUS / 3), differenceOfScreen);
            //// the coordinates (x,y) of the end point after curve
            //mFirstCurveEndPoint.Set(mNavigationBarWidth / 2, CURVE_CIRCLE_RADIUS + (CURVE_CIRCLE_RADIUS / 4) + differenceOfScreen);
            //// same thing for the second curve
            //mSecondCurveStartPoint = mFirstCurveEndPoint;
            //mSecondCurveEndPoint.Set((mNavigationBarWidth / 2) + (CURVE_CIRCLE_RADIUS * 2) + (CURVE_CIRCLE_RADIUS / 3), differenceOfScreen);

            //// the coordinates (x,y)  of the 1st control point on a cubic curve
            //mFirstCurveControlPoint1.Set(mFirstCurveStartPoint.X + CURVE_CIRCLE_RADIUS + (CURVE_CIRCLE_RADIUS / 4), mFirstCurveStartPoint.Y);
            //// the coordinates (x,y)  of the 2nd control point on a cubic curve
            //mFirstCurveControlPoint2.Set(mFirstCurveEndPoint.X - (CURVE_CIRCLE_RADIUS * 2) + CURVE_CIRCLE_RADIUS, mFirstCurveEndPoint.Y);

            //mSecondCurveControlPoint1.Set(mSecondCurveStartPoint.X + (CURVE_CIRCLE_RADIUS * 2) - CURVE_CIRCLE_RADIUS, mSecondCurveStartPoint.Y);
            //mSecondCurveControlPoint2.Set(mSecondCurveEndPoint.X - (CURVE_CIRCLE_RADIUS + (CURVE_CIRCLE_RADIUS / 4)), mSecondCurveEndPoint.Y);

            var x1 = (mNavigationBarWidth - CURVE_CIRCLE_RADIUS * 2) / 2;
            var r = CURVE_CIRCLE_RADIUS / 2;
            var R = CURVE_CIRCLE_RADIUS;

            mFirstCurveStartPoint.Set(x1 - r/3, differenceOfScreen);
            mFirstCurveEndPoint.Set(x1 + r, differenceOfScreen + r + r/3);

            mSecondCurveStartPoint = mFirstCurveEndPoint;
            mSecondCurveEndPoint.Set(x1 + R + 2*r/3, differenceOfScreen);

            mFirstCurveControlPoint1.Set(x1 + r, differenceOfScreen);
            mFirstCurveControlPoint2.Set(x1 + r, differenceOfScreen + r + r/3);

            mSecondCurveControlPoint1.Set(x1 + r + r / 2, differenceOfScreen + r + r/3);
            mSecondCurveControlPoint2.Set(x1 + r + r / 2, differenceOfScreen);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            mPath.Reset();
            mPath.MoveTo(0, differenceOfScreen);
            mPath.LineTo(mFirstCurveStartPoint.X, mFirstCurveStartPoint.Y);

            mPath.CubicTo(mFirstCurveControlPoint1.X, mFirstCurveControlPoint1.Y,
                    mFirstCurveControlPoint2.X, mFirstCurveControlPoint2.Y,
                    mFirstCurveEndPoint.X, mFirstCurveEndPoint.Y);

            mPath.CubicTo(mSecondCurveControlPoint1.X, mSecondCurveControlPoint1.Y,
                    mSecondCurveControlPoint2.X, mSecondCurveControlPoint2.Y,
                    mSecondCurveEndPoint.X, mSecondCurveEndPoint.Y);

            mPath.LineTo(mNavigationBarWidth, differenceOfScreen);
            mPath.LineTo(mNavigationBarWidth, mNavigationBarHeight);
            mPath.LineTo(0, mNavigationBarHeight);
            mPath.Close();

            canvas.DrawPath(mPath, mPaint);

            Paint circlePaint = new Paint();
            circlePaint.SetStyle(Paint.Style.FillAndStroke);
            circlePaint.Color = Android.Graphics.Color.LightBlue;

            canvas.DrawCircle(300, 300, 50, circlePaint);
        }
    }
}